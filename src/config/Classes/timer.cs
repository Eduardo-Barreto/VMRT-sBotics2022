/// <summary>
/// Gerencia o tempo.
/// </summary>

public class myTimer{
    /// <summary>
    /// Armazena o tempo inicial do robô em milissegundos
    /// </summary>
    public long startTime;

    /// <summary>
    /// Construtor da classe
    /// </summary>
    public myTimer(){
        this.startTime = this.currentUnparsed;
    }

    /// <summary>
    /// Tempo atual em milissegundos desde 1970
    /// </summary>
    public long currentUnparsed{
        get{
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }

    /// <summary>
    /// Tempo atual em milissegundos desde o início da rotina
    /// </summary>
    public long current{
        get{
            return this.currentUnparsed - this.startTime;
        }
    }

    /// <summary>
    /// Reseta o timer atual
    /// </summary>
    /// <param name="_startTime">(long) Valor alvo para resetar o timer</param>
    public void resetTimer(long _startTime = 0){
        this.startTime = this.current + _startTime;
    }

    /// <summary>
    /// Espera um tempo em milissegundos
    /// </summary>
    /// <param name="milliseconds">(int) Tempo a esperar</param>
    public async Task delay(int milliseconds = 50){
        await Time.Delay(milliseconds);
    }

    /// <summary>
    /// Espera um tempo em milissegundos enquanto realiza uma função.
    /// </summary>
    /// <param name="milliseconds">(int) Tempo a esperar</param>
    /// <param name="doWhileWait">(função) Ação para fazer enquanto espera</param>
    public void delay(int milliseconds, ActionHandler doWhileWait){
        long timeout = this.current + (milliseconds/50);
        while(current < timeout){
            doWhileWait();
            this.delay();
        }
    }
}
