
// For sending
export interface OrderItem{
  item_id: number;
  item_count: number;
  price: number;
  name: string;
  image: string;
  attribute: {
    name: string;
    value: {
      name: string;
      price: number;
    }[]
  }[];
}

export interface RestaurantOrder{
  total_price: number;
  paymentItems: OrderItem[];
  restaurantId: number;
}

export interface Cart{
  total_price: number;
  orders: RestaurantOrder[]
}

