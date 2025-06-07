using HtmlAgilityPack;
using HTMLParser;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        // Check if the user provided a file path
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: HtmlParser <path-to-html-file>");
            return;
        }

        // Get the file path from the arguments
        string filePath = args[0];

        // Check if the file exists
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: The file '{filePath}' does not exist.");
            return;
        }

        // Load the HTML document from the file
        string html;
        try
        {
            html = File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading the file: {ex.Message}");
            return;
        }
        var htmlDoc = new HtmlDocument();
        List<BidProposalItem> bidProposalItems = new List<BidProposalItem> { };
        htmlDoc.LoadHtml(html);

        // data-cy="proposal-items-list"
        var sectionNodes = htmlDoc.DocumentNode.SelectNodes("//div[@data-cy='proposal-items-list']");

        Console.WriteLine("Extracted nodes with data-cy=\"proposal-items-list\":");
        if (sectionNodes != null)
        {
            foreach (var section in sectionNodes)
            {
                //Console.WriteLine(section.OuterHtml);

                // Extract child nodes with tag <cw-bidx-proposal-item> and data-cy="proposal-item"
                var proposalItemNodes = section.SelectNodes(".//cw-bidx-proposal-item[@data-cy='proposal-item']");
                Console.WriteLine("Extracted <cw-bidx-proposal-item> nodes with data-cy=\"proposal-item\":");

                if (proposalItemNodes != null)
                {
                    foreach (var proposalItem in proposalItemNodes)
                    {
                        string lineNumber = "", itemNumber = "", description = "", quantity = "", unit = "";

                        // /html/body/div/div/cw-new-bidx-app-layout/div/section/section/div[2]/cw-bidx-proposal-item-section[1]/cw-bidx-proposal-item[1]/div/div/div[1]/div[1]/div[1]
                        //var target = proposalItem.SelectNodes("//*[@id=\"app__bidx\"]/cw-new-bidx-app-layout/div/section/section/div[2]/cw-bidx-proposal-item-section[1]/cw-bidx-proposal-item[1]/div/div/div[1]/div[1]/div[1]");
                        var target = proposalItem.SelectNodes(".//div/div/div[1]/div[1]/div[1]");
                        foreach (var LineNumberNode in target)
                        {
                            lineNumber = LineNumberNode.InnerHtml.Split(" ")[1];
                        }
                        target = proposalItem.SelectNodes(".//div/div/div[1]/div[1]/div[2]");
                        foreach (var ItemNumberNode in target)
                        {
                            itemNumber = ItemNumberNode.InnerHtml;
                        }

                        target = proposalItem.SelectNodes(".//div/div/div[1]/div[2]");
                        try
                        {
                            description = ReplaceMultipleSpacesWithSingleSpace(target[1].InnerHtml.Replace("&amp;", "&"));
                        }
                        catch
                        {

                        }
                        target = proposalItem.SelectNodes(".//div/div/div[2]/div/div[1]");
                        foreach (var QuantityNode in target)
                        {
                            quantity = QuantityNode.InnerHtml;

                        }

                        target = proposalItem.SelectNodes(".//div/div/div[2]/div/div[2]");
                        foreach (var UnitNode in target)
                        {
                            unit = UnitNode.InnerHtml;
                        }

                        var bidProposalItem = new BidProposalItem(lineNumber, itemNumber, description, quantity, unit);
                        bidProposalItems.Add(bidProposalItem);
                    }
                }
                else
                {
                    Console.WriteLine("No matching <cw-bidx-proposal-item> nodes found.");
                }
            }
        }
        else
        {
            Console.WriteLine("No matching nodes found.");
        }

        // TODO: use these to create the CSV
        foreach (var item in bidProposalItems)
        {
            Console.WriteLine(item);
        }
    }
    public static string ReplaceMultipleSpacesWithSingleSpace(string input)
    {
        // Define the regex pattern to match sequences of two or more spaces
        string pattern = @"\s{2,}";

        // Replace matched sequences with a single space
        string result = Regex.Replace(input, pattern, " ");

        return result;
    }
}
