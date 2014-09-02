// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CurrentUserSteps.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Specs.Steps
{
    using TechTalk.SpecFlow;

    [Binding, Scope(Tag = "currentUser")]
    public class CurrentUserSteps : StepDefinitionBase
    {
        [Given(@"Я гость")]
        public void ДопустимЯГость()
        {
            var logoutButton = this.I.Find("a[href='/account/logout']");

            if (logoutButton.Exists())
            {
                this.I.Click(logoutButton);
            }

            this.I.Assert.Exists("a[dialog='/account/login']");
        }

        [Given(@"Я под пользователем ""(.*)"" и мой пароль ""(.*)""")]
        public void ДопустимЯПодПользователемИМойПароль(string userName, string password)
        {
            var profileLink = I.Find(GetProfileSectionSelector(userName));

            if (!profileLink.Exists())
            {
                var logoutButton = I.Find("a[href='/account/logout']");
                if (logoutButton.Exists())
                {
                    I.Click(logoutButton);
                }

                I.Click("body > header a:contains('Войти')");
                I.WaitUntil(() => I.Assert.Exists(".dialog-body > h1:contains('Войти')"));

                this.EnterTextInInput("E-mail или логин", userName);
                this.EnterTextInInput("Пароль", password);

                I.Click(".dialog-body .dialog-buttons a:contains('Войти')");
                I.WaitUntil(() => I.Assert.Not.Exists(".ajax-request-executing"), 10);

                I.Assert.Exists(profileLink);
            }
        }

        private void EnterTextInInput(string fieldName, string text)
        {
            var inputName = this.I.Find(string.Format("label:contains('{0}')", fieldName)).Element.Attributes.Get("for");
            this.I.Enter(text).In(string.Format("input[name='{0}']", inputName));
        }

        [Then(@"Меня перебрасывает на главную")]
        public void ТоМеняПеребрасываетНаГлавную()
        {
            I.WaitUntil(() => this.I.Expect.Url("https://local.mirgames.ru/"), 5);
            I.Assert.Url("https://local.mirgames.ru/");
        }

        [Then(@"Я под пользователем ""(.*)""")]
        public void ТоЯПодПользователем(string userName)
        {
            I.Assert.Exists(GetProfileSectionSelector(userName));
        }

        [Then(@"Я гость")]
        public void ТоЯГость()
        {
            I.Assert.Not.Exists("a[href='/account/logout']");
        }

        [Then(@"Пользователь ""(.*)"" среди пользователей онлайн")]
        public void ТоПользовательСредиПользователейОнлайн(string userName)
        {
            I.Assert.Exists(string.Format(".online-users a[title='{0}']", userName));
        }

        private static string GetProfileSectionSelector(string userName)
        {
            return string.Format("body > header .profile-section span:contains('{0}')", userName);
        }
    }
}
