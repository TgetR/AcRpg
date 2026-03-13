
# AcRpg (Action Role-Playing Game)

**AcRpg** — это ролевая 2D экшен-игра с видом сверху, разрабатываемая на Unity. В рамках этого проекта я сфокусировался на проектировании масштабируемых игровых систем (Core Gameplay Mechanics), написании чистого кода и создании гибкой архитектуры.

## Технические особенности и реализованные системы

Вся логика, архитектура и механики, описанные ниже, спроектированы и написаны мной на C#:

### 1. Внутриигровая консоль и парсинг команд (Developer Console)

Для удобства тестирования и отладки реализована собственная скрытая консоль. Система принимает строковый ввод, разбивает его на аргументы и выполняет соответствующие методы (выдача квестов, очистка лога, эхо-вывод).


```
string ConsoleExecute(string message)
{
    string[] input = message.Split(' ');
    switch (input[0].ToLower())
    {
        case "quest":
            if (input.Length > 1)
            {
                switch (input[1].ToLower())
                {
                    case "complete":
                        questChecker.CompleteQuest();
                        return "Quest completed.";
                    case "finish":
                        questChecker.DeleteQuest();
                        return "Quest finished.";
                }
            }
            return "Usage: quest [complete/finish]";
        // ... другие команды
    }
}

```

### 2. Управление окружением и рендерингом (Environment & URP)

Реализована система плавного перехода между биомами (например, "Холодный лес"). Скрипты динамически управляют глобальным 2D-освещением (`Light2D`) и переключают профили Post-Processing (`Volume`), а также накладывают на игрока статусные эффекты с помощью корутин.



```
public void ColdForestEnable()
{
    volumeDefault.GetComponent<Volume>().enabled = false;
    volumeForest.GetComponent<Volume>().enabled = true;
    GlobalLight.GetComponent<Light2D>().intensity = 0.1f;
}

```

### 3. Продвинутая боевая система и физика (Combat & Physics)

-   **Оптимизированный поиск целей:** Использование `HashSet` для отслеживания врагов в радиусе атаки исключает дубликаты и снижает нагрузку на память.
    
-   **Система отбрасывания (Knockback):** Векторная физика на основе `Rigidbody2D` с расчетом нормализованного направления от источника урона и асинхронным таймером оглушения.
    


```
public void KnockBack(Transform player, float force, float stunTime)
{
    _isKnockedBack = true;
    _chasingController.moveRestrict = true;
    
    // Расчет вектора отбрасывания от игрока
    Vector2 direction = (transform.position - player.position).normalized;
    _rb.linearVelocity = direction * force;
    
    StartCoroutine(KnockBackCounter(stunTime));
}

```

### 4. Data-Driven Дизайн и Экономика

-   Архитектура предметов и баффов/дебаффов построена на `ScriptableObjects`. Это полностью отделяет базу данных предметов от логики кода.
    
-   Система динамической генерации витрины магазина с алгоритмом псевдослучайного перемешивания (Shuffle) и строгими проверками баланса игрока.
    

### 5. Система UI-уведомлений (On-Screen Notifications)

Гибкий менеджер всплывающих уведомлений, поддерживающий уровни важности (от обычных инфо-сообщений до критических ошибок) с автоматической сменой цвета и триггером анимаций.


```
public void Notify(string text, int Level)
{
    Color UsingColor = Color.navyBlue; // Значение по умолчанию
    switch (Level)
    {
        case 1: UsingColor = Color.softGreen; break; // Успех
        case 3: UsingColor = Color.softRed; break;   // Ошибка
        case 4: UsingColor = Color.crimson; break;   // Критическое предупреждение
    }
    Text.color = UsingColor;
    Text.text = text;
    animator.SetTrigger("Notify");
}

```

## Стек технологий

-   **Движок:** Unity 6000.2.7f2 (Universal Render Pipeline)
    
-   **Язык:** C# (.NET)
    
-   **Паттерны и структуры данных:** Data-Driven Design (Scriptable Objects), `HashSet`, Coroutines, Singleton.
    


## Лицензия

* **Исходный код:** Весь написанный мной код (скрипты, архитектура, системы) распространяется под лицензией **[MIT](https://opensource.org/licenses/MIT)**. Вы можете свободно использовать, модифицировать и распространять его.
* **Игровые ассеты:** Все визуальные и аудио материалы проекта, не защищенные иным авторским правом, предоставляются на условиях лицензии **[CC BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/)**. 
* Ассеты сторонних авторов используются и распространяются согласно их собственным лицензиям.
