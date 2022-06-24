using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain.Views;

namespace WorkShopUI.Transformers
{
    public abstract class BaseTransformer
    {
        public static PageableView BuildPageable<T>(SearchResponse<T> searchResponse)
        {
            return new PageableView
            {
                First = searchResponse.First,
                Last = searchResponse.Last,
                PageSize = searchResponse.Size,
                NumberOfElements = searchResponse.NumberOfElements,
                TotalElements = searchResponse.TotalElements,
                TotalPages = searchResponse.TotalPages
            };
        }

        protected static string GetId(ResourceObject resourceObject) {

            return resourceObject.Links != null ? GetIdFromLinks(resourceObject.Links)
                : GetIdFromSelfLink(resourceObject.SelfLink);
        }

        protected static string GetIdFromSelfLink(SelfLink selfLink)
        {
            if ((selfLink != null) && (selfLink.Self != null))
            {
                var self = selfLink.Self.Href;
                var index = self.LastIndexOf("/");

                return self.Substring(index + 1);
            }

            return "";
        }

        protected static string GetIdFromLinks(IEnumerable<Link> links)
        {
            if (links != null)
            {
                var self = links.Filter(link => link.Rel.Contains("self"))
                    .Select(link => link.Href)
                    .FirstOrDefault("");

                var index = self.LastIndexOf("/");

                return self.Substring(index + 1);
            }

            return "";
        }


    }
}