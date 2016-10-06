using System;
using NUnit.Framework;

[TestFixture]
public class TestAmazonWebService {
    [Test]
    public void TestSearch() {
        AWSECommerceService service = new AWSECommerceService();
        //service.Url = "http://soap.amazon.com/onca/soap?Service=AWSECommerceService";
        service.Url = "http://localhost:8080/onca/soap?Service=AWSECommerceService";

        ItemSearchRequest search = new ItemSearchRequest();
        search.Keywords = "television lcd";
        search.SearchIndex = "Electronics";
        search.ResponseGroup = new string[] { "Large" };
        //OperationRequest 
        ItemSearch request = new ItemSearch();
        request.Request = new ItemSearchRequest[] { search };
        request.Validate = "false";
        request.XMLEscaping = "Single";
        request.SubscriptionId = DeveloperIds.AmazonID;
        ItemSearchResponse response = service.ItemSearch( request);
        Console.WriteLine( response.OperationRequest.ToString());
        if( response.Items.Length > 0 ) {
            Items[] arrayItems = response.Items;
            foreach( Items items in arrayItems ) {
                foreach( Item item in items.Item ) {
                    Console.WriteLine( "Title (" + item.ItemAttributes.Title + ")" );
                    Console.WriteLine( "Item (" + item.ASIN + ")" );
                }
            }
        }
    }
    [Test]
    public void TestFilter() {
        Devspace.Commons.PipesFilters.Chain<Devspace.Commons.PipesFilters.StreamingControlImpl< Item>> chain =
            new Devspace.Commons.PipesFilters.Chain<Devspace.Commons.PipesFilters.StreamingControlImpl<Item>>();

        chain.AddLink( Chap04.Application.PipesFilters.Factory.CreateSearchTV( "lcd" ) );
        chain.AddLink( Chap04.Application.PipesFilters.Factory.CreateFilterPrice( 1000, 2000 ) );
        chain.AddLink( Chap04.Application.PipesFilters.Factory.CreateReviewRating( 4.0 ) );
        chain.AddLink( Chap04.Application.PipesFilters.Factory.CreateOutput() );

        Devspace.Commons.PipesFilters.StreamingControlImpl<Item> controldata =
            new Devspace.Commons.PipesFilters.StreamingControlImpl<Item>();
        chain.Process( controldata );
    }

 }

