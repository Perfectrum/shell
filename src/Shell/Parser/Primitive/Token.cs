namespace Shell.Parser.Primitive;

using Shell.Expression;
using System.Collections.Generic;

/// <summary>
///     Всевозможные типы токенов
/// </summary>
public enum TokenType
{
    T_WORD,
    T_WS,
    T_ASS,
    T_ASS_PRED,
    T_CMD,
    T_EQ,
    T_TMP,
    T_COR,
    T_ERR
}

/// <summary>
///     Представляет собой базовый класс для токенов,
///     которые участвуют в парсинге выражения
/// </summary>
abstract public class Token
{
    /// <summary>
    ///     Тип токена
    /// </summary>
    public TokenType Type { get; protected set; }
    /// <summary>
    ///     Оригинальная строка из которой сконструирован данный токен
    /// </summary>
    public string Original { get; set; } = "";
    /// <summary>
    ///     Метод, который делает reduce текущего стэка и возвращает новый токен
    /// </summary>
    /// <param name="x">Стэк, на текущий момент парсинга</param>
    /// <returns>Новый токен или null, если новый токен сконструировать невозможно</returns>
    abstract public Token? Join(Stack<Token> x);
    /// <summary>
    ///     Превращает текущий токен в выражение для исполнения, 
    ///     если токен является конечным состоянием. ResultError в противном случае.
    /// </summary>
    /// <returns>Выражение для вычисления в монаде Result</returns>
    abstract public Result<Expression> Render();

#if DEBUG
    /// <summary>
    ///     Строковое представление данного токена 
    ///     в формате удобном для debug'а и тестирования
    /// </summary>
    abstract public string ToDebugString();
#endif
}
