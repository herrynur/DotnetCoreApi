namespace BackendService.Helper.Swagger
{
    [AttributeUsage(AttributeTargets.Property |
        AttributeTargets.Class |
        AttributeTargets.Struct |
        AttributeTargets.Parameter |
        AttributeTargets.Enum,
        AllowMultiple = true)]
    public class SwaggerSchemaExampleAttribute : Attribute
    {
        public SwaggerSchemaExampleAttribute(string example)
        {
            Example = example;
        }

        public string Example { get; }
    }
}
