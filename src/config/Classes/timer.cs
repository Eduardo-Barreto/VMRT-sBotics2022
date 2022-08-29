/**
 * @brief Gerencia o tempo.
 *
 */

public static class timer{
    /**
	 * @brief Armazena o tempo inicial do robô em milissegundos
	 *
	 */
    public static long startTime;

    public static void init(){
        startTime = currentUnparsed;
    }

    /**
	 * @brief Tempo atual em milissegundos desde 1970
	 */
    public static long currentUnparsed{
        get{
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }

    /**
	 * @brief Tempo atual em milissegundos desde o início da rotina
	 */
    public static long current{
        get{
            return currentUnparsed - startTime;
        }
    }

    /**
	 * @brief Reseta o timer atual
	 *
	 * @param _startTime: (long) Valor alvo para resetar o timer
	 */
    public static void resetTimer(long _startTime = 0){
        startTime = current + _startTime;
    }

    /**
	 * @brief Espera um tempo em milissegundos
	 *
	 * @param milliseconds: (int) Tempo a esperar
	 */
    public static async Task delay(int milliseconds = 1){
        await Time.Delay(milliseconds);
    }

    /**
	 * @brief Espera um tempo em milissegundos enquanto realiza uma função.
	 *
	 * @param milliseconds: (int) Tempo a esperar
	 * @param doWhileWait: (função) Ação para fazer enquanto espera
	 */
    public static async Task delay(int milliseconds, ActionHandler doWhileWait){
        long timeout = current + milliseconds;
        while(current < timeout){
            doWhileWait();
            await delay();
        }
    }
}