using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain;

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

        protected static string GetId(IEnumerable<Link> links)
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