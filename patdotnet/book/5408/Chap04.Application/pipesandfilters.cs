using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Devspace.Commons.TDD;
using Devspace.Commons.PipesFilters;

namespace Chap04.Application.PipesFilters {
    internal class SearchTV : InputSink<Item> {
        private string _keywords;

        public SearchTV( string keywords ) {
            _keywords = keywords;
        }
        public override void Process( OutputStream<Item> output ) {
            AWSECommerceService service = new AWSECommerceService();
            //service.Url = "http://soap.amazon.com/onca/soap?Service=AWSECommerceService";
            service.Url = "http://localhost:8080/onca/soap?Service=AWSECommerceService";

            ItemSearchRequest search = new ItemSearchRequest();
            search.Keywords = _keywords + " tv";
            search.SearchIndex = "Electronics";
            search.ResponseGroup = new string[] { "Large" };
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
                        output.Write( item );
                    }
                }
            }
        }
    }

    internal class FilterPrice : ComponentInputIterator<Item> {
        private double _minimum;
        private double _maximum;

        public FilterPrice( double maximum, double minimum) {
            _maximum = maximum;
            _minimum = minimum;
        }
        public override void Process( Item node, OutputStream< Item> output) {
            double price = Double.Parse( node.ItemAttributes.ListPrice.Amount );
            if( price >= _minimum && price <= _maximum) {
                output.Write( node);
            }
        }
    }

    internal class ReviewRating : ComponentInputIterator<Item> {
        private Decimal _minRating;

        public ReviewRating( double rating ) {
            _minRating = new Decimal( rating);
        }
        public override void Process( Item node, OutputStream<Item> output ) {
            if( Decimal.Compare( _minRating, node.CustomerReviews.AverageRating) < 0.0) {
                output.Write( node );
            }
        }
    }

    internal class Output : OutputSink<Item> {
        public override void Process( InputStream<Item> input ) {
            foreach( Item item in input ) {
                // Do something in ASP.NET!
            }
        }
    }

    public static class Factory {
        public static IComponent<StreamingControlImpl< Item>> CreateSearchTV( string keywords) {
            return (IComponent<StreamingControlImpl< Item>>)new SearchTV( keywords);
        }
        public static IComponent<StreamingControlImpl<Item>> CreateFilterPrice( double maximum, double minimum) {
            return (IComponent<StreamingControlImpl<Item>>)new FilterPrice( maximum, minimum );
        }
        public static IComponent<StreamingControlImpl<Item>> CreateReviewRating( double rating) {
            return (IComponent<StreamingControlImpl<Item>>)new ReviewRating( rating );
        }
        public static IComponent<StreamingControlImpl<Item>> CreateOutput() {
            return (IComponent<StreamingControlImpl<Item>>)new Output();
        }
    }
}

/*
     private void doSearch( String endpoint) throws Exception {
        AWSECommerceServiceLocator locator = new AWSECommerceServiceLocator();
        AWSECommerceServicePortType service = locator.getAWSECommerceServicePort( new URL( endpoint));

        ItemSearchRequest request = new ItemSearchRequest();
        request.setAuthor( "Christian Gross");
        request.setSearchIndex( "Books");
        _OperationRequestHolder opResultHolder = new _OperationRequestHolder();
        ItemsArrayHolder resultHolder = new ItemsArrayHolder();
        service.itemSearch( samples.IdentifierKeys.AmazonKey, "", "Single", "false",
                            request, null, opResultHolder, resultHolder);
        if( resultHolder.value.length > 0) {
            _Items[] items = resultHolder.value;

            for( int c1 = 0; c1 < items.length; c1 ++) {
                _Item[] item = items[ c1].getItem();
                for( int c2 = 0; c2 < item.length; c2 ++) {
                    System.out.println( "Item (" + item[ c2].getItemAttributes().getTitle() + ")");
                    System.out.println( "ASIN (" + item[ c2].getASIN() + ")");
                }
            }
        }
    }
 public void doSpecificSearch( String[] asin, String endpoint) throws Exception {
        AWSECommerceServiceLocator locator = new AWSECommerceServiceLocator();
        AWSECommerceServicePortType service = locator.getAWSECommerceServicePort( new URL( endpoint));

        ItemLookupRequest request = new ItemLookupRequest();
        request.setItemId( asin);
        _OperationRequestHolder opResultHolder = new _OperationRequestHolder();
        ItemsArrayHolder resultHolder = new ItemsArrayHolder();
        service.itemLookup( samples.IdentifierKeys.AmazonKey, "", "false", "Single",
                            request, null, opResultHolder, resultHolder);
        if( resultHolder.value.length > 0) {
            _Items[] items = resultHolder.value;

            for( int c1 = 0; c1 < items.length; c1 ++) {
                _Item[] item = items[ c1].getItem();
                for( int c2 = 0; c2 < item.length; c2 ++) {
                    System.out.println( "Item (" + item[ c2].getItemAttributes().getTitle() + ")");
                    System.out.println( "ASIN (" + item[ c2].getASIN() + ")");
                }
            }
        }
    }
 *
 * */
