using MagicConchShellMaui.Models;
using System.Diagnostics;

namespace MagicConchShellMaui.Tools
{
    public class DecideAnswer
    {
        private readonly AudioPlayer _audioPlayer;

        public DecideAnswer()
        {
            _audioPlayer = new AudioPlayer();
        }

        private List<KnownQuestion> _knownQuestions = new List<KnownQuestion>();

        /// <summary>
        /// Wenn eine gestellte Fragen kürzer als 3 Wörte ist, ist es keine Frage...
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private bool IsQuestion(string question)
        {
            var words = question.Split(' ');
            if (words.Length >= 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sucht in der Datenbank nach der gestellten Frage. Wenn es diese gibt, wird sie zurückgegeben. 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private KnownQuestion WasAlreadyAsked(string question)
        {
            var questionresult = _knownQuestions.Where(x => x.Question == question.ToLower()).FirstOrDefault();

            return questionresult;
        }

        /// <summary>
        /// Die KI prüft, ob es sich bei der gestellten Frage um eine offene oder um eine ja/nein Frage handelt.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private bool IsDecisionQuestion(string question)
        {
            YesNoAI.ModelInput modelInput = new YesNoAI.ModelInput()
            {
                QuestionLower = question,
            };

            try
            {
                var result = YesNoAI.Predict(modelInput);

                int resultint = Convert.ToInt32(result.PredictedLabel);

                //0 = Du Offen
                //1 = Du Ja/Nein
                //2 = Ich Ja/Nein
                //3 = Ich Offen

                if (resultint == 0 || resultint == 3)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Prüft, ob das wort 'oder' in der Frage enthalten ist. 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private bool IsOpenQuestion(string question)
        {
            return question.ToLower().Contains("oder");
        }

        /// <summary>
        /// Wertet die Frage aus und gibt eine Audio-Antwort aus.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public async Task Evaluate(string question)
        {
            if (string.IsNullOrEmpty(question))
            {
                return;
            }

            question = question.ToLower();

            if (question[question.Length - 1] != '?')
            {
                question += "?";
            }

            var knownQuestion = WasAlreadyAsked(question);

            if (!IsQuestion(question))
            {
                _audioPlayer.Play(AudioPlayer.Audios.FragDochEinfachNochmal);
            }
            else if (knownQuestion != null)
            {
                if (knownQuestion.Answer == AudioPlayer.Audios.Nein)
                {
                    _audioPlayer.Play(AudioPlayer.Audios.Neein);
                }
                else
                {
                    _audioPlayer.Play(knownQuestion.Answer);
                }
            }
            else if (IsOpenQuestion(question))
            {
                _audioPlayer.Play(AudioPlayer.Audios.KeinsVonBeiden);
            }
            else if (IsDecisionQuestion(question))
            {
                AudioPlayer.Audios decidedAnswer;
                int randomNum = new Random().Next(0, 10);
                if (randomNum <= 5)
                {
                    if (randomNum <= 2)
                    {
                        decidedAnswer = AudioPlayer.Audios.IchGlaubeEherNicht;
                    }
                    else
                    {
                        decidedAnswer = AudioPlayer.Audios.Nein;
                    }
                }
                else
                {
                    decidedAnswer = AudioPlayer.Audios.Ja;
                }

                _knownQuestions.Add(new KnownQuestion
                    {
                        Question = question,
                        Answer = decidedAnswer,
                    });

                _audioPlayer.Play(decidedAnswer);
            }
            else
            {
                _audioPlayer.Play(AudioPlayer.Audios.FragDochEinfachNochmal);
            }
        }
    }
}