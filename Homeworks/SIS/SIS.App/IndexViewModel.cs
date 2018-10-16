namespace SIS.App
{
    using Framework.Attributes.Property;

    public class IndexViewModel
    {
        [NumberRange(5,12)]
        public double Id { get; set; }

        [Regex("^[^0-9]+$")]
        public string Username { get; set; }
    }
}
