﻿#language:ru
@currentUser
@menu
@chat

Функционал: Страница Чат

Контекст: 
	Допустим Я открыл MirGames
		И Я под пользователем "user-test" и мой пароль "qqq111"
		И Я нажал на "Чат" в меню
		И Дождался загрузки чата

Сценарий: Отправка сообщения в чате
	Когда Я ввожу сообщение "Отправка сообщения в чате" в чате
		И Нажимаю кнопку "Отправить"
	То Сообщение с текстом "Отправка сообщения в чате" появляется в списке сообщений

Сценарий: Отправка сообщения в чате по нажатию на enter
	Допустим Быстрая клавиша для отправки сообщений - это "ENTER"
		И Фокус в текстовом поле чата
	Когда Я ввожу сообщение "Отправка сообщения в чате по нажатию на enter" в чате
		И Нажимаю клавишу "ENTER"
	То Сообщение с текстом "Отправка сообщения в чате по нажатию на enter" появляется в списке сообщений

Сценарий: Отправка пустого сообщения в чате невозможна
	Допустим Фокус в текстовом поле чата
		И Текстовое поле чата пустое
	Когда Я нажимаю кнопку "Отправить"
	То Сообщение без текста не появляется в списке сообщений

Сценарий: Отправка пустого сообщения в чате невозможна по нажатию на enter
	Допустим Быстрая клавиша для отправки сообщений - это "ENTER"
		И Фокус в текстовом поле чата
		И Текстовое поле чата пустое
	Когда Нажимаю клавишу "ENTER"
	То Сообщение без текста не появляется в списке сообщений

Сценарий: Редактирование сообщения в чате
	Допустим Я отправил сообщение "тестовое сообщение" в чате
		И Фокус в текстовом поле чата
		И Текстовое поле чата пустое
	Когда Я нажимаю клавишу "UP"
		И Сообщение с текстом "тестовое сообщение" загружается для редактирования
	То Я ввожу сообщение "отредактированное сообщение" в чате
		И Нажимаю кнопку "Сохранить"
	Тогда Сообщение с текстом "отредактированное сообщение" появляется в списке сообщений