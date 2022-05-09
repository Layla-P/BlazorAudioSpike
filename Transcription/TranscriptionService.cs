using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Transcription.Common.Dtos;
using Transcription.Common.Dtos.RequestModels;
using Transcription.Helpers.Interfaces;

namespace Transcription
{
    public class TranscriptionService : ITranscriptionService
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceHelpers _helper;
        private readonly TranscriptionAccount _account;


        public TranscriptionService(IOptions<TranscriptionAccount> transcriptionAccount, HttpClient httpClient, IServiceHelpers helper)
        {
            _account = transcriptionAccount.Value ?? throw new ArgumentNullException(nameof(transcriptionAccount));
            _helper = helper ?? throw new ArgumentNullException(nameof(helper));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.DefaultRequestHeaders.Add("Authorization", _account.AuthToken);
        }
        public async Task<TranscriptionResponse> SubmitAudioFileAsync(TranscriptionRequest transcriptionRequest)
        {
            return await _helper.SubmitAsync(transcriptionRequest);
        }
        public async Task<TranscriptionResponse> RetrieveAudioFileAsync(string transcriptId)
        {
            _httpClient.DefaultRequestHeaders.Add("Accepts", "application/json");
            
            HttpResponseMessage response = await _httpClient.GetAsync($"{_account.Endpoint}/{transcriptId}");

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var resp = _helper.ConvertToTranscriptionResponse(responseJson);

            return resp;
        }


    }
}
