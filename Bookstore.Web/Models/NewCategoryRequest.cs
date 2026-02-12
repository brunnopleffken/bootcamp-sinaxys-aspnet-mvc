using System.ComponentModel.DataAnnotations;

namespace Bookstore.Web.Models;

public record NewCategoryRequest(
    [Required(ErrorMessage = "O nome da categoria é obrigatório")]
    [MinLength(2, ErrorMessage = "O nome da categoria deve ter no mínimo 2 caracteres")]
    string Name);
