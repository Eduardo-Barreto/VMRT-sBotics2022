static double map(double val, double inMin, double inMax, double outMin, double outMax)
{
    // "mapeia" ou reescala um val (val), de uma escala (inMin~inMax) para outra (outMin~outMax)
    return (val - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
}

static bool interval(double val, double min, double max)
{
    // verifica se um valor (val) está dentro de um intervalo (min~max)
    return val >= min && val <= max;
}

static bool proximity(double val, double target, double tolerance = 1)
{
    // verifica se um valor (val) está proximo de um alvo (target) com tolerância (tolerance)
    return interval(val, target - tolerance, target + tolerance);
}

static int convertDegrees(double degrees){
    // converte um angulo em graus para sempre se manter entre 0~360
    return (int)((degrees % 360 + 360) % 360);
}

static bool degreesProximity(double degrees, double target, double tolerance = 1){
    // verifica se um angulo (degrees) está proximo de um alvo (target) com tolerância (tolerance)
    return interval(
        convertDegrees(degrees),
        convertDegrees(target - tolerance),
        convertDegrees(target + tolerance)
    );
}

static double constrain(double val, double min, double max)
{
    // limita um valor (val) entre um intervalo (min~max)
    return val < min ? min : val > max ? max : val;
}