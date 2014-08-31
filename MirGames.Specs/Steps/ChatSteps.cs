// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ChatSteps.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Specs.Steps
{
    using MirGames.Infrastructure;

    using TechTalk.SpecFlow;

    [Binding, Scope(Tag = "chat")]
    public class ChatSteps : StepDefinitionBase
    {
        [Given(@"Дождался загрузки чата")]
        public void ДопустимДождалсяЗагрузкиЧата()
        {
            I.WaitUntil(() => I.Assert.Not.Exists(".socket-notification div:visible"));
        }

        [When(@"Я ввожу сообщение ""(.*)"" в чате")]
        [Then(@"Я ввожу сообщение ""(.*)"" в чате")]
        public void ЕслиЯВвожуСообщениеВЧате(string text)
        {
            I.Enter(text).In("form[name='postAnswerForm'] textarea");
        }

        [Then(@"Сообщение без текста не появляется в списке сообщений")]
        public void ТоСообщениеБезТекстаНеПоявляетсяВСпискеСообщений()
        {
            I.Assert.True(() => I.Find("article.message.own-message:last .text").Element.Text.Trim() != string.Empty);
        }

        [Given(@"Быстрая клавиша для отправки сообщений - это ""(.*)""")]
        public void ДопустимБыстраяКлавишаДляОтправкиСообщений_Это(string key)
        {
            var sendKey = I.Find("a.button.sendKey");
            
            if (sendKey.Element.Text.Trim() == "Ctrl+Enter" && key.EqualsIgnoreCase("Enter"))
            {
                I.Click(sendKey);
            }

            if (sendKey.Element.Text.Trim() == "Enter" && key.EqualsIgnoreCase("Ctrl+Enter"))
            {
                I.Click(sendKey);
            }
        }

        [When(@"Нажимаю кнопку ""(.*)""")]
        [Then(@"Нажимаю кнопку ""(.*)""")]
        [When(@"Я нажимаю кнопку ""(.*)""")]
        [Then(@"Я нажимаю кнопку ""(.*)""")]
        public void ЕслиНажимаюКнопку(string name)
        {
            I.Click(string.Format("form[name='postAnswerForm'] a:contains('{0}')", name));
        }

        [Then(@"Сообщение с текстом ""(.*)"" появляется в списке сообщений")]
        public void ТоСообщениеСТекстомПоявляетсяВСпискеСообщений(string text)
        {
            I.Assert.True(() => I.Find("article.message.own-message:last .text").Element.Text.Trim() == text);
        }

        [Given(@"Я отправил сообщение ""(.*)"" в чате")]
        public void ДопустимЯНедавноОтправилСообщениеВЧате(string text)
        {
            I.Enter(text).In("form[name='postAnswerForm'] textarea");
            I.Click("form[name='postAnswerForm'] a:contains('Отправить')");
        }

        [Given(@"Фокус в текстовом поле чата")]
        public void ДопустимФокусВТекстовомПолеЧата()
        {
            I.Focus("form[name='postAnswerForm'] textarea");
        }

        [Given(@"Текстовое поле чата пустое")]
        public void ДопустимТекстовоеПолеЧатаПустое()
        {
            I.Enter(string.Empty).In("form[name='postAnswerForm'] textarea");
        }

        [When(@"Я нажимаю клавишу ""(.*)""")]
        [When(@"Нажимаю клавишу ""(.*)""")]
        public void ЕслиЯНажимаюКлавишуВверх(string key)
        {
            I.Press("{" + key + "}");
        }

        [When(@"Сообщение с текстом ""(.*)"" загружается для редактирования")]
        public void ЕслиСообщениеСТекстомЗагружаетсяДляРедактирования(string text)
        {
            I.WaitUntil(() => I.Find("form[name='postAnswerForm'] textarea").Element.Value == text);
        }

    }
}
