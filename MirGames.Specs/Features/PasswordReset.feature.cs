﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34014
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace MirGames.Specs.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Восстановление пароля")]
    [NUnit.Framework.CategoryAttribute("currentUser")]
    [NUnit.Framework.CategoryAttribute("dialog")]
    [NUnit.Framework.CategoryAttribute("forms")]
    [NUnit.Framework.CategoryAttribute("menu")]
    public partial class ВосстановлениеПароляFeature : FluentAutomation.FluentTest
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PasswordReset.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Восстановление пароля", "", ProgrammingLanguage.CSharp, new string[] {
                        "currentUser",
                        "dialog",
                        "forms",
                        "menu"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
            ScenarioContext.Current[ScenarioContext.Current.ScenarioInfo.Title] = this;
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 9
#line 10
 testRunner.Given("Я открыл MirGames", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 11
 testRunner.And("Я гость", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 12
 testRunner.When("Я нажимаю кнопку меню \"Войти\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 13
 testRunner.And("Открывается диалог \"Войти\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
 testRunner.And("Я нажимаю кнопку диалога \"Забыли пароль?\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
 testRunner.And("Открывается диалог \"Восстановление пароля\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Пользователь указал верный логин")]
        public virtual void ПользовательУказалВерныйЛогин()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Пользователь указал верный логин", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 18
 testRunner.When("Я ввожу \"user-test\" в поле \"E-mail или логин\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 19
  testRunner.And("Ввожу \"qqq11\" в поле \"Новый пароль\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 20
  testRunner.And("Нажимаю кнопку диалога \"Восстановить\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
 testRunner.Then("В диалоге выводиться текст \"Инструкции по восстановлению высланы на указанный адр" +
                    "ес электронной почты\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Пользователь указал неверный логин")]
        public virtual void ПользовательУказалНеверныйЛогин()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Пользователь указал неверный логин", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 24
 testRunner.When("Я ввожу \"user-test-wrong\" в поле \"E-mail или логин\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 25
  testRunner.And("Ввожу \"qqq11\" в поле \"Новый пароль\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 26
  testRunner.And("Нажимаю кнопку диалога \"Восстановить\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 27
 testRunner.Then("В диалоге выводиться предупреждение \"Неверные логин или пароль\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Пользователь не ввёл логин")]
        public virtual void ПользовательНеВвёлЛогин()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Пользователь не ввёл логин", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 30
 testRunner.When("Я ввожу \"qqq11\" в поле \"Новый пароль\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 31
 testRunner.Then("Кнопка диалога \"Восстановить\" недоступна", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Пользователь не ввёл пароль")]
        public virtual void ПользовательНеВвёлПароль()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Пользователь не ввёл пароль", ((string[])(null)));
#line 33
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 34
 testRunner.When("Я ввожу \"user-test\" в поле \"E-mail или логин\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 35
 testRunner.Then("Кнопка диалога \"Восстановить\" недоступна", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Пользователь передумал")]
        public virtual void ПользовательПередумал()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Пользователь передумал", ((string[])(null)));
#line 37
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 38
 testRunner.When("Я нажимаю кнопку диалога \"Отмена\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 39
 testRunner.Then("Диалог \"Восстановление пароля\" закрывается", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
