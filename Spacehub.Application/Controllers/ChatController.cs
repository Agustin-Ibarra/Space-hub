using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Models;
using SpaceHub.Application.Repository;

namespace SpaceHub.Application.Controllers;

public class ChatController : Controller
{
  private readonly IChatRepository _chatRepository;

  public ChatController(IChatRepository chatRepository)
  {
    _chatRepository = chatRepository;
  }
  
  [Authorize]
  [HttpGet]
  [Route("/chat")]
  public IActionResult Chat()
  {
    return View();
  }

  [Authorize]
  [HttpPost]
  [Route("/api/chat")]
  public async Task<IActionResult> ApiChatGenerate([FromBody] PromptDto prompt)
  {
    if (!ModelState.IsValid)
    {
      var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
      return BadRequest(new { error = errors });
    }
    else
    {
      int tokensAprox = prompt.UserPrompt.Length / 4; // validar la cantidad de tokens
      if (tokensAprox > 150) // si sobrepasa los 150 tokens consume repidamente el plan gratuito
      {
        return BadRequest(new { error = "el prompt supera los 150 tokens" });
      }
      else
      {
        using (var httpClient = new HttpClient()) // crea una instancia temporal al salir del bloque el objeto se destruye
        {
          var url = $"{Environment.GetEnvironmentVariable("URL_ENDPOINT")}?key={Environment.GetEnvironmentVariable("GEMINI_API_KEY")}";
          // definir reglas para generar las respuestas
          string systemInstruction = @"
Eres un asistente amigable y experto en temas relacionados con el cosmos, astronomía y el universo.
Solo debes responder preguntas sobre el espacio, planetas, estrellas, agujeros negros, asteroides, galaxias u otros fenómenos astronómicos.
Si el usuario hace una pregunta fuera de esos temas, o una pregunta sin sentido, responde con un mensaje breve, educado y comprensible,
por ejemplo: 'Lo siento, solo puedo responder preguntas relacionadas con el cosmos.'";
          var payload = new // esquema del objeto que espera la API de gemini
          {
            contents = new[]
            {
              new {
                role = "user",
                parts = new[]
                {
                  new { text = $"{systemInstruction}\n\nPregunta del usuario: {prompt.UserPrompt}" }
                }
              }
            },
            generationConfig = new
            {
              temperature = 0.7, // configurar por un tono calido en la respuesta
              topP = 0.9,
              maxOutputTokens = 200 // configurar tokens maximo de salida en la respuesta
            }
          };

          var payloadJson = JsonConvert.SerializeObject(payload); // convertir el objeto a JSON con serializacion
          var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

          var response = await httpClient.PostAsync(url, content);
          response.EnsureSuccessStatusCode();

          var responseString = await response.Content.ReadAsStringAsync();
          var responseObject = JsonConvert.DeserializeObject<GeminiResponseDto>(responseString); // desearilazr JSON a objeto de C#
          if (responseObject != null)
          {
            var generatedText = responseObject;
            var geminiRespone = generatedText.candidates[0].content.parts[0].text;
            var chatData = new ChatMessage
            {
              id_chat = prompt.idChat,
              ia_message = geminiRespone,
              user_message = prompt.UserPrompt,
              date_chat = DateTime.Now,
            };
            await _chatRepository.AddChat(chatData);
            return Created("/api/chat", new { response = geminiRespone });
          }
          else
          {
            return StatusCode(500, new { error = "Ocurrio un error al generar la respuesta" });
          }
        }
      }
    }
  }
}