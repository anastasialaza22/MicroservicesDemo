namespace OrderService.Data
{
    public static class DbInitializer
    {
        public static void Initialize(OrderContext context)
        {
            if (context.Orders.Any())
            {
                return; // Данные уже инициализированы
            }

            // Можно добавить инициализацию заказов, если необходимо
        }
    }
}