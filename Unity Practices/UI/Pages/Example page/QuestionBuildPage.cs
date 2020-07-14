using QuizLogic;
using UnityEngine;
using TMPro;
using GameCore.Data;
using System.IO;

namespace GameCore
{
    namespace Menu
    {
        public class QuestionBuildPage : Page
        {
            public static QuestionBuildPage Instance;

            [SerializeField] private TMP_InputField _questionLabel;
            [SerializeField] private TMP_InputField _numberOfAnswers;
            [SerializeField] private TMP_InputField _secondsForAnswer;
            [SerializeField] private TMP_Dropdown _answerType;
            //TODO: Add Image reference

            private AnswerType _answerTypeInCurrentQuestion;
            private int _numberOfAnswersInCurrentQuestion;

            private Quiz _newQuiz;
            private int _addedQuestionsCount;

            private string _charPool = "abcdefghijklmnopqrstuvwxyz1234567890";

            public GameObject[] AnswersPool;

            public int QuestionCount { get; set; }
            
            private void Awake()
            {
                Instance = this;
            }

            public void CreateQuiz(string name)
            {
                _newQuiz = new Quiz(name);
                _addedQuestionsCount = 0;
                _numberOfAnswersInCurrentQuestion = 0;
                _answerTypeInCurrentQuestion = AnswerType.TextAnswer;
                _numberOfAnswers.transform.parent.gameObject.SetActive(false);
            }

            public void OnNumerOfAnswersChanged()
            {
                int number;
                
                if (int.TryParse(_numberOfAnswers.text, out number))
                {
                    if (number > 0 && number < 9)
                    {
                        if (_numberOfAnswersInCurrentQuestion < number)
                        {
                            for (int i = 0; i < number; i++)
                            {
                                AnswersPool[i].SetActive(true);
                            }
                        }
                        else
                        {
                            for (int i = number; i < _numberOfAnswersInCurrentQuestion; i++)
                            {
                                AnswersPool[i].SetActive(false);
                            }
                        }

                        _numberOfAnswersInCurrentQuestion = number;
                        return;
                    }
                }

                HideAllAnswers();
            }

            public void OnAnswerTypeChanged()
            {
                _answerTypeInCurrentQuestion = (AnswerType)_answerType.value;
                if (_answerType.value != 0)
                {
                    _numberOfAnswers.transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    _numberOfAnswers.transform.parent.gameObject.SetActive(false);
                }
            }

            public void OnContinueClicked()
            {
                int timeInSeconds;
                bool timeIsValid = int.TryParse(_secondsForAnswer.text, out timeInSeconds);

                if (timeIsValid == false)
                {
                    return;
                }

                if (_numberOfAnswersInCurrentQuestion <= 0 || _numberOfAnswersInCurrentQuestion >= 9)
                {
                    return;     
                }

                Question questionTemplate = new Question(_answerTypeInCurrentQuestion, _questionLabel.text, timeInSeconds);
                questionTemplate.QuestionIndex = _addedQuestionsCount;

                //set answers
                for (int i = 0; i < _numberOfAnswersInCurrentQuestion; i++)
                {
                    questionTemplate.AddAnswer(AnswersPool[i].GetComponentInChildren<TMP_InputField>().text);
                }

                _newQuiz.AddQuestion(questionTemplate);

                _addedQuestionsCount++;
                _numberOfAnswersInCurrentQuestion = 0;


                //Reset All Fields to default
                HideAllAnswers();
                _questionLabel.text = string.Empty;
                _numberOfAnswers.text = string.Empty;
                _secondsForAnswer.text = string.Empty;
                _answerType.value = 0;

                if (_addedQuestionsCount == QuestionCount)
                {
                    PageController.Instance.TurnPageOn(PageType.QuizInformationPage, true, () =>
                    {
                        QuizInformationPage quizInformationPage = QuizInformationPage.Instance;
                        quizInformationPage.isStudentInfo = false;

                        //Generate quiz identificator && quiz info
                        string identificator = new string(GenerateQuizIdentificator(6));
                        //TODO: mb check that id is unic
                        quizInformationPage.SetText("Новый тест был создан успешно. Код доступа - " + identificator);
                        print(Application.persistentDataPath + "/" + "quiz" + ".qz");
                        //Save Quiz txt
                        SaveSystem.SaveData(_newQuiz, "quiz"+identificator);
                        string path = SaveSystem.GetFormatPath("quiz" + identificator);
                        EmailActions.SendInfo("QuizId",identificator,path);
                      
                    });
                    
                }

            }

            private void HideAllAnswers()
            {
                foreach (var answerGO in AnswersPool)
                {
                    answerGO.GetComponentInChildren<TMP_InputField>().text = string.Empty;
                    answerGO.SetActive(false);
                }
            }

            private char[] GenerateQuizIdentificator(int length)
            {
                char[] identificator = new char[length];

                for (int i = 0; i < length; i++)
                {
                    identificator[i] = _charPool[Random.Range(0, _charPool.Length)];
                }

                return identificator;
            }
        }
    }
}
