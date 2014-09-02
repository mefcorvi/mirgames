// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DialogSteps.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Specs.Steps
{
    using TechTalk.SpecFlow;

    [Binding, Scope(Tag = "dialog")]
    public class DialogSteps : StepDefinitionBase
    {
        [When(@"ќткрываетс€ диалог ""(.*)""")]
        public void ≈сли∆дуќткрыти€ƒиалога(string name)
        {
            I.WaitUntil(() => this.I.Expect.Exists(string.Format(".dialog-body > h1:contains('{0}')", name)));
        }

        [Then(@"¬ диалоге выводитьс€ предупреждение ""(.*)""")]
        public void “о¬ƒиалоге¬ыводитьс€ѕредупреждение(string text)
        {
            I.Assert.Exists(string.Format(".dialog-body .error:contains('{0}')", text));
        }

        [When(@"(?:[я€]\s+?)?[Ќн]ажимаю кнопку диалога ""(.*)""")]
        public void ≈слиЌажимаю нопкуƒиалога(string name)
        {
            I.Click(string.Format(".dialog-body .dialog-buttons a:contains('{0}')", name));
            I.WaitUntil(() => this.I.Expect.Not.Exists(".ajax-request-executing"), 10);
        }
        
        [Then(@"ƒиалог ""(.*)"" закрываетс€")]
        public void “оƒиалог«акрываетс€(string name)
        {
            I.Assert.Not.Exists(string.Format(".dialog-body > h1:contains('{0}')", name));
        }

        [Then(@"¬ диалоге выводитьс€ текст ""(.*)""")]
        public void “о¬ƒиалоге¬ыводитьс€“екст(string text)
        {
            I.Assert.Exists(string.Format(".dialog-body div:contains('{0}')", text));
        }

        [Then(@" нопка диалога ""(.*)"" недоступна")]
        public void “о нопкаƒиалогаЌедоступна(string name)
        {
            I.Expect.Class("button-disabled").On(string.Format(".dialog-body .dialog-buttons a:contains('{0}')", name));
        }
    }
}