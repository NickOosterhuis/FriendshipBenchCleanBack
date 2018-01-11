using MobileApp.Models;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using MobileApp.Helpers;

namespace MobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuestionnairePage : ContentPage
	{
        private APIRequestHelper apiRequestHelper;
        private List<Question> allQuestions;
        private List<Answers> allAnswers = new List<Answers>();
        private int questionNumber = -1;
        private int numberOfYesQuestions = 0;
        private bool suicideQuestion;

        // Initialize the page.
        public QuestionnairePage ()
		{
            apiRequestHelper = new APIRequestHelper();
			InitializeComponent ();
            FetchQuestions();
        }

        // Actions for when the user clicks 'Yes'.
        private void button_yes(object sender, EventArgs e)
        {
            SaveAnswer("Yes");
            numberOfYesQuestions++;
            if (allQuestions[questionNumber].Id == 11)
            {
                suicideQuestion = true;
            }
            ShowQuestionOrComplete();
        }

        // Actions for when the user clicks 'Yes'.
        private void button_no(object sender, EventArgs e)
        {
            SaveAnswer("No");
            ShowQuestionOrComplete();
        }

        // Fetch all the questions and make a list of it.
        private async Task FetchQuestions()
        {
            // Send token with Http request
            apiRequestHelper.SetTokenHeader();
            // Send a GET request to the API.
            string apiResponse = await apiRequestHelper.GetRequest(Constants.questionsUrl);
            if (apiResponse != null)
            {
                // Convert the API response into a JSON object.
                allQuestions = new List<Question>();
                dynamic convertedJson = JsonConvert.DeserializeObject(apiResponse);

                // Loop through all the appointments in the JSON object and create appointment objects.
                foreach (var question in convertedJson)
                {
                    allQuestions.Add(new Question { Id = (int)question.id, QuestionString = (string)question.question } );
                }

                // Show the first question.
                ShowQuestionOrComplete();
            }
            else
            {
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }
        }

        // Show the next question or complete the questionnaire
        private void ShowQuestionOrComplete()
        {
            questionNumber++;
            if (questionNumber < allQuestions.Count)
            {
                ProgressLabel.Text = "Question " + (questionNumber + 1) + " out of " + allQuestions.Count;
                QuestionLabel.Text = allQuestions[questionNumber].QuestionString;
            }
            else
            {
                YesButton.IsEnabled = false;
                NoButton.IsEnabled = false;
                CompleteQuestionnaire();
            }
        }

        // Save an answer to the current question.
        private void SaveAnswer(string answer)
        {
            allAnswers.Add(new Answers { Answer = answer, Question_id = allQuestions[questionNumber].Id } );
        }

        // Complete the questionnaire.
        private async void CompleteQuestionnaire()
        {
            // Create a new questionnaire model.
            Questionnaire questionnaire = new Questionnaire
            {
                Client_id = (string)App.Current.Properties["id"],
                Time = DateTime.Now,
                Redflag = (numberOfYesQuestions > 8 || suicideQuestion)
            };

            
            // Do a POST request.
            string content = JsonConvert.SerializeObject(questionnaire);
            // Send token with Http request
            apiRequestHelper.SetTokenHeader();
            string response = await apiRequestHelper.PostRequest(Constants.questionnaireUrl, content);
            if (response != null)
            {
                
                // Add the new questionnaire ID to all the local answers.
                dynamic convertedJson = JsonConvert.DeserializeObject(response);
                foreach (var answer in allAnswers)
                {
                    answer.Questionnaire_id = (int)convertedJson.id;
                }

                // Do a POST request.
                string answerContent = JsonConvert.SerializeObject(allAnswers);
                // Send token with Http request
                apiRequestHelper.SetTokenHeader();
                string answerResponse = await apiRequestHelper.PostRequest(Constants.answerUrl, answerContent);
                if (response != null)
                {
                    await DisplayAlert("Completed", "You have completely filled out the questionnaire.", "Okay");
                    Navigation.PushAsync(new MainQuestionnaire());
                }
                else
                {
                    // Display an error.
                    DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
                }
            }
            else
            {
                // Display an error.
                DisplayAlert("Error", "Sorry, something went wrong. Please try again later.", "Okay");
            }
        }
    }
}