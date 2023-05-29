-> main
===main===
Привет, Игрок! #speaker:Василий #portrait:vasiliy #layout:default
Слышал от коллег, что ты теперь работаешь в отделе лицензионного контроля в Министерстве образования.
Слушай, у меня ведь супруга владеет сетью детских садов, а через 2 месяца у нас плановая проверка по соблюдению лицензионный требований. Скажу прямо - предлагаю тебе для наших сотрудников провести ряд платных консультаций по подготовке к проверке. В любое удобно время! Считай, хороший дополнительный заработок.
-> enter
===enter===
Ну, что скажешь? Когда удобно встретиться?
    + [Приветствую! Без вопросов, проведу все без лишних формальностей.]
        -> wrong()
    + [Приветствую! С большим удовольствием, мне только необходимо соблюсти одну формальность - уведомить своего руководителя о такой работе.]
        -> wrong()
    + [Приветствую! Выполнять иную оплачиваемую работу мне можно, но если это влечет за собой конфликт интересов. Здесь полагаю, он очевиден…]
        -> correctFirst()
===correctFirst()===
Конфликт интересов? Да брось! Здесь нет ничего такого. В чем этот конфликт интересов здесь может быть? Это ведь всего лишь консультация! Вы такие и у себя на службе проводите каждый день. #layout:big
    + [В данном случае мне важно соблюсти только порядок уведомления об условиях работы. Уведомительный порядок направления информации о намерении осуществлять иную оплачиваемую работу не требует получения согласия от моего руководства. Запретить мне выполнять иную оплачиваемую работу не вправе!]
        -> wrong()
    + [В данном случае такая ситуация может трактоваться как личная заинтересованность, которая может привести к конфликту интересов. Это моя ответственность со всеми вытекающими из этого юридическими последствиями. Извини, но я вынужден отказаться от твоего предложения.]
        -> correctSecond()
===correctSecond()===
И как мне быть теперь? Не ожидал я твоего отказа….
    + [Попробуй выйти с таким предложением на моих коллег по отделу, может быть кто-то согласиться, сейчас… поищу контакты.]
        -> wrong()
    + [Специалисты нашего отдела оказывают консультационную помощь - есть установленные консультационные часы, также периодически мы проводим открытые информационные семинары, а на нашем сайте размещено много по твоему вопросу методических рекомендаций. Посмотрите, пожалуйста, информацию на сайте. Кроме того, есть возможность всегда обратиться с устным или письменным обращением, на которое специалисты подготовят официальный ответ.]
        -> correctThird()
===correctThird()===
Эх, ну ладно, спасибо за помощь!#layout:default
-> END
===wrong()===
#note:wrong3
-> main