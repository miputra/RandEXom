// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom


namespace RandEXom.Interface
{
    
    public interface ISeedR
    {
        long init { get;}
        long now { get;}
        long previous { get; }
        void Next();
    }
}
