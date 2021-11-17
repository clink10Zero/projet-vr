public enum BlocDirection
{
    Haut,
    Avant,
    Droite,
    Bas,
    Arrier,
    Gauche
}

public static class BlocDirectionExtension
{
    public static BlocDirection Next(BlocDirection v)
    {
        return v == BlocDirection.Gauche ? v = BlocDirection.Haut : v + 1;
    }

    public static BlocDirection Previous(BlocDirection v)
    {
        return v == BlocDirection.Haut ? v = BlocDirection.Gauche : v - 1;
    }

    public static BlocDirection Opposite(BlocDirection v)
    {
        return (int)v < 3 ? v + 3 : v - 3;
    }
} 