namespace OmniumCase.DTOs;

public class CartResponse

{
    public List<ApiCart> Carts { get; set; } = new();
}

public class ApiCart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Total { get; set; }
    public List<ApiCartProduct> Products { get; set; } = new();
}

public class ApiCartProduct
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class UserResponse
{
    public List<ApiUser> Users { get; set; } = new();
}

public class ApiUser
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}