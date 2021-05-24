namespace Modelos
{
    public class GeneralResponse<T>
    {
        public int error { get; set; }

        public string mensaje { get; set; }

        public T rta { get; set; }

        public GeneralResponse(T data, string message, int errors)
        {
            this.error = errors;
            this.mensaje = message;
            this.rta = data;
        }
    }
}
