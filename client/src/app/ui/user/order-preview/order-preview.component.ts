import { Component, OnInit } from '@angular/core';
import { RestaurantOrder } from 'src/app/models/Order.model';
import { CartService, MyOrdersResponse } from 'src/app/services/cart.service';

@Component({
  selector: 'app-order-preview',
  templateUrl: './order-preview.component.html',
  styleUrls: ['./order-preview.component.css']
})
export class OrderPreviewComponent implements OnInit {

  orders: MyOrdersResponse = {data: []};

  constructor(private cartService: CartService) { }

  ngOnInit(): void {
    this.cartService.getMyOrders()
      .subscribe(result => {
        console.log(result);

        this.orders.data = result.data;
        this.orders.data.forEach(el => {
          el.paymentItems = JSON.parse(el.paymentItems);
        })
      });

    // for(let big of this.allOrders){
    //   const keys = Object.keys(big.orders)
    //   this.vendorUsernames.push(keys);

    //   let temp_orders = [];
    //   for (let key of keys) {
    //     temp_orders.push(big.orders[key]);
    //   }
    //   this.orders.push(temp_orders);
    // }

  }

}
