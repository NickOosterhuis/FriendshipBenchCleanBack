using MobileApp.Helpers;
using MobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionnaireDetailPage : ContentPage
    {
        APIRequestHelper apiRequestHelper;
        QuestionnaireGetViewModel questionnaire;
        int questionnaireId;

        // Initialize the page.
        public QuestionnaireDetailPage(int questionnaireId)
        {
            apiRequestHelper = new APIRequestHelper();
            this.questionnaireId = questionnaireId;
            FetchQuestionnaire();
            InitializeComponent();
        }

        // Fetch the Questionnaire.
        private async Task FetchQuestionnaire()
        {
            // Send token with Http request
            apiRequestHelper.SetTokenHeader();
            // Send a GET request to the API.
            string apiResponse = await apiRequestHelper.GetRequest(Constants.questionnaireUrl + "/" + questionnaireId);
            if (apiResponse != null)
            {
                // Convert the API response into a JSON object.
                dynamic convertedJson = JsonConvert.DeserializeObject(apiResponse);

                // Save all the answers.
                List<AnswerGetViewModel> answers = new List<AnswerGetViewModel>();
                dynamic answersJson = convertedJson.answers;
                foreach (var answer in answersJson)
                {
                    answers.Add(new AnswerGetViewModel
                    {
                        QuestionId = (int)answer.questionId,
                        Question = (string)answer.question,
                        Answer = (string)answer.answer
                    });
                }

                // Create a new object from the questionnaire.
                questionnaire = new QuestionnaireGetViewModel
                {
                    Time = (DateTime)convertedJson.time,
                    Client_id = (string)convertedJson.client_id,
                    Redflag = (bool)convertedJson.redflag,
                    Answers = answers
                };
            }
            else
            {
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }

            // Update the view.
            UpdateView();
        }

        // Update the view.
        private void UpdateView()
        {
            Header.Text = questionnaire.DateTime;
            Redflag.Text = questionnaire.RedflagString;
            foreach(AnswerGetViewModel answer in questionnaire.Answers)
            {
                Label questionLabel = new Label { Text = answer.Question, FontAttributes = FontAttributes.Bold };
                Label answerLabel = new Label { Text = answer.Answer };
                StackPanel.Children.Add(questionLabel);
                StackPanel.Children.Add(answerLabel);
            }
        }
    }
}