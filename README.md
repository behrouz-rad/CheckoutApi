# CheckoutApi

This is a prototype in ASP.NET Core 2.0 environment by which customers can manage a basket of items.

Also there is a console application to demonstrate how the endpoints work.

To run, two projects (Checkout.Web and Checkout.ApiClient.Console) should be run simultaneously respectively.

PS: I used MemoryCache in this project just for the purpose of the demo. REST by design is stateless. By adding session (or anything else of that kind) we makes it stateful and defeating any purpose of having a RESTful API.
So in a real project, a possible scenario would be obtaining a token from the server as soon as the customer starts choosing an item into his/her basket. This token can be used to distinguish the customer and his/her items in something like Distributed Cache.
Another possible solution is using localStorage.
