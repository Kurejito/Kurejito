namespace Kurejito.Validation {
    ///<summary>
    ///</summary>
    public interface IValidate<T> {
        /// <summary>
        ///   Validates the specified t.
        /// </summary>
        /// <param name = "t">The t.</param>
        /// <returns></returns>
        ValidationResult Validate(T t);
    }
}