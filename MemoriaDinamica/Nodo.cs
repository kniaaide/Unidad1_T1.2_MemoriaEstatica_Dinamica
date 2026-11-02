namespace Unidad1.MemoriaDinamica
{
    /// <summary>
    /// Unidad de almacenamiento dinámico. Cada instancia se crea en el Heap
    /// mediante el operador new y se enlaza con otras mediante referencias.
    /// </summary>
    public class Nodo
    {
        public int Valor { get; set; }

        // Referencia al siguiente nodo en el Heap (null si es el último).
        public Nodo? Siguiente { get; set; }

        public Nodo(int valor)
        {
            Valor = valor;
            Siguiente = null;
        }
    }
}
