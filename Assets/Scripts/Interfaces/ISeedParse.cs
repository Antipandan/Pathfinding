using System;

public interface ISeedParse
{
    public Random GetRandom { get; }

    public int ParseSeed(string seedString);

}
