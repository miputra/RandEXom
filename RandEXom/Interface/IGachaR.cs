// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom


namespace RandEXom.Interface
{
    public interface IGachaR<T>
    {
        /// <summary>
        /// Count every item in pool
        /// </summary>
        /// <returns></returns>
        int Count();
        /// <summary>
        /// Count for specific item in pool
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        int Count(T type);
        /// <summary>
        /// Counting how many type in pool
        /// </summary>
        /// <returns></returns>
        int CountType();
        void AddItem(T item, int count);
        /// <summary>
        /// make the current item pool become init pool
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isIterative"></param>
        void RemoveEmpty(T item);

        /// <summary>
        /// set current pool to init pool
        /// </summary>
        /// <param name="isIterative"></param>
        void Refill();
        void Remove(T value);
        T Pull();
    }
}
