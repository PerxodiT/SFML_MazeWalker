using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFMLMazeWalker
{
    class QuestionDrawer : Drawable
    {
        Question Question { get; set; }

        public QuestionDrawer(Question question)
        {
            Question = question;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {

        }
    }

    class Question 
    {
        public string Text { get; set; }
        public string[] Answers { get; set; }
        public int RightAnswerID { get; set; }
    }

    static class Qustions
    {
        static Random r = new Random(DateTime.Now.Millisecond);
        static private Question[] questions = new Question[] {
            new Question
            {
                Text = "Вопрос 0?",
                Answers = new string[] {
                "Ответ 0",
                "Ответ 1",
                "Ответ 2",
                "Ответ 3"
                },
                RightAnswerID = 0
            },
            new Question
            {
                Text = "Вопрос 1?",
                Answers = new string[] {
                "Ответ 0",
                "Ответ 1",
                "Ответ 2",
                "Ответ 3"
                },
                RightAnswerID = 1
            },
            new Question
            {
                Text = "Вопрос 2?",
                Answers = new string[] {
                "Ответ 0",
                "Ответ 1",
                "Ответ 2",
                "Ответ 3"
                },
                RightAnswerID = 2
            },
            new Question
            {
                Text = "Вопрос 3?",
                Answers = new string[] {
                "Ответ 0",
                "Ответ 1",
                "Ответ 2",
                "Ответ 3"
                },
                RightAnswerID = 3
            }
        };
        static Question TakeRandom()
        {
            return questions[r.Next(questions.Length)];
        }
    }
}
