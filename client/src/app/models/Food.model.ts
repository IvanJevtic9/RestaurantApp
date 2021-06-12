export interface attribute {
  name: string;
  should_add_on_price: boolean; // If true, attribute price shoud be added on dish price
  multiple_select: boolean; // More than one attribut could be selected
  values: {
    name: string;
    price: number;
  }[];
}

export interface Dish{
  name: string;
  image?: string;
  ingredients_list: string;
  attributes: attribute[];
  price: number;
}

export interface Menu{
  restaurant_id: number,
  name: string;
  dishes: Dish[];
}

