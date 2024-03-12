/*  [S] - Single Responsibility Principle - принцип единственной ответственности. 
 *  [O] - Open closed Principle - принцип открытости-закрытости
 *  [L] - Liskov substitution Principle - принцип подстановки Барбары Лисков.
 *  [I] - Interface Segregation Principle - принцип разделения интерфейсов.
 *  [D] - Dependency Inversion Principle - принцип инверсии зависимостей. 
 */
using System;

class Program
{
    static void Main()
    {
        // S: Принцип единой ответственности -->
        var generator = new RandomNumberGenerator();
        var validator = new NumberValidator();
        // S: Принцип единой ответственности <--


        GameManager gameManager;
        Console.WriteLine("Выберете уровень сложности: 1 - Easy, 2 - Hard");
        int choice = Int32.Parse(Console.ReadLine());
        switch (choice)
        {
            // D: Принцип инверсии зависимости
            case 1:
                gameManager = new EasyModeSettings(generator, validator);
                break;
            case 2:
                gameManager = new HardModeSettings(generator, validator);
                break;
            default:
                Console.WriteLine("Выбрать нужно 1 или 2");
                return;
        }
        gameManager.StartGame();

    }
}

// D: Принцип разделения интерфейса -->
public interface INumberGenerator
{
    int GenerateNumber(int min, int max);
}

public interface INumberValidator
{
    bool IsValid();
    bool CompareNumber(int checkNumber);
    void SetNumber(int randomNumber);

}

public interface IHintSender
{
    string GetHint();
}
// D: Принцип разделения интерфейса <--

public class RandomNumberGenerator : INumberGenerator
{
    private Random random = new Random();
    public int GenerateNumber(int min, int max)
    {
        return random.Next(min, max);
    }
}

public class NumberValidator : INumberValidator
{
    private int _randomNumber;
    private bool _isValid;
    public bool CompareNumber(int currentNumber)
    {
        if (currentNumber > _randomNumber)
        {
            Console.WriteLine("Меньше");
        }
        else if (currentNumber < _randomNumber)
        {
            Console.WriteLine("Больше");
        }
        else
        {
            _isValid = true;
        }
        return _isValid;
    }

    public bool IsValid()
    {
        return _isValid;
    }

    public void SetNumber(int randomNumber)
    {
        _randomNumber = randomNumber;
    }
}



public abstract class GameManager
{
    private readonly INumberGenerator numberGenerator;
    private readonly INumberValidator numberValidator;
    public virtual int GetMin()
    {
        return 0;
    }
    public virtual int GetMax()
    {
        return 100;
    }
    public virtual int GetLife()
    {
        return 10;
    }
    public virtual string WelcomeMessage()
    {
        return "";
    }

    public GameManager(INumberGenerator generator, INumberValidator validator)
    {
        numberGenerator = generator;
        numberValidator = validator;
    }

    public void StartGame()
    {
        int min = GetMin();
        int max = GetMax();
        int life = GetLife();

        int targetNumber = numberGenerator.GenerateNumber(min, max);
        Console.WriteLine(WelcomeMessage());
        numberValidator.SetNumber(targetNumber);
        int attempts = 0;
        int guess;

        Console.WriteLine($"Угадайте число между {min} и {max}");

        do
        {
            if (life <= 0)
            {
                Console.WriteLine("В следующий раз повезет!");
                break;
            }

            Console.Write($"Введите вашу догадку: (у вас {life} попыток)");
            
            guess = int.Parse(Console.ReadLine());

            numberValidator.CompareNumber(guess);
            attempts++;
            life--;
        }
        while (!numberValidator.IsValid());

        if (life > 0)
        {
            Console.WriteLine($"Поздравляю, вы угадали число с {attempts} попыток!");
        }
        Console.ReadKey();
    }
}


// O: Принцип Открытости-закрытости
// L: Принцип подстановки Барбары Лисков

// Уровень сложности - сложный
public class HardModeSettings : GameManager
{
    public override int GetMin()
    {
        return 0;
    }
    public override int GetMax()
    {
        return 200;
    }
    public override int GetLife()
    {
        return 8;
    }

    public override string WelcomeMessage()
    {
        return "Вы выбрали сложный уровень";
    }

    public HardModeSettings(INumberGenerator generator, INumberValidator validator) : base(generator, validator)
    {

    }
}

// O: Принцип Открытости-закрытости
// L: Принцип подстановки Барбары Лисков

// Уровень сложности - легкий
public class EasyModeSettings : GameManager
{
    public override int GetMin()
    {
        return 0;
    }
    public override int GetMax()
    {
        return 100;
    }
    public override int GetLife()
    {
        return 12;
    }

    public override string WelcomeMessage()
    {
        return "Вы выбрали легкий уровень";
    }

    public EasyModeSettings(INumberGenerator generator, INumberValidator validator) : base(generator, validator)
    {

    }
}