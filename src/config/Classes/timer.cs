/// <summary>
/// Gerencia o tempo.
/// </summary>

public static class timer{
    /// <summary>
    /// Armazena o tempo inicial do robô em milissegundos
    /// </summary>
    public static long startTime;

    /* /// <summary>
    /// Construtor da classe
    /// </summary>
    public static customTimer(int _startTime = 0){
        startTime = currentUnparsed + _startTime;
    } */

    public static void init(){
        startTime = currentUnparsed;
    }

    /// <summary>
    /// Tempo atual em milissegundos desde 1970
    /// </summary>
    public static long currentUnparsed{
        get{
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }

    /// <summary>
    /// Tempo atual em milissegundos desde o início da rotina
    /// </summary>
    public static long current{
        get{
            return currentUnparsed - startTime;
        }
    }

    /// <summary>
    /// Reseta o timer atual
    /// </summary>
    /// <param name="_startTime">(long) Valor alvo para resetar o timer</param>
    public static void resetTimer(long _startTime = 0){
        startTime = current + _startTime;
    }

    /// <summary>
    /// Espera um tempo em milissegundos
    /// </summary>
    /// <param name="milliseconds">(int) Tempo a esperar</param>
    public static async Task delay(int milliseconds = 50){
        await Time.Delay(milliseconds);
    }

    /// <summary>
    /// Espera um tempo em milissegundos enquanto realiza uma função.
    /// </summary>
    /// <param name="milliseconds">(int) Tempo a esperar</param>
    /// <param name="doWhileWait">(função) Ação para fazer enquanto espera</param>
    public static async Task delay(int milliseconds, ActionHandler doWhileWait){
        long timeout = current + milliseconds;
        while(current < timeout){
            doWhileWait();
            await delay();
        }
    }
}