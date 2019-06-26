using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface
    {
        public void Render(CountryInfo[] countries, IEnumerable<Link> links)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowPosition(0, 0);
            Console.Clear();

            RenderLinks(links);
            RenderCountries(countries);
        }

        private void RenderCountries(CountryInfo[] countries)
        {
            foreach (var country in countries)
            {
                RenderCountry(country);
            }
        }

        private void RenderLinks(IEnumerable<Link> links)
        {
            foreach (var link in links)
            {
                var link1 = link;
                var link2 = new Link(link.Neighbour, link.Country, link.LinkTypes);

                RenderLinkToRemoteNeighbour(link);
                RenderLinkToRemoteNeighbour(link2); // for indirect links
            }
        }

        private void RenderLinkToRemoteNeighbour(Link link)
        {
            var links = new Link[]
            {   new NorthLink(link.Country, link.Neighbour, link.LinkTypes),
                new SouthLink(link.Country, link.Neighbour, link.LinkTypes),
                new EastLink(link.Country, link.Neighbour, link.LinkTypes),
                new WestLink(link.Country, link.Neighbour, link.LinkTypes)
            };

            var requiredLink = links
                .Where(l => l.IsRequiredDirection()).First()
                .EvaluateParameters();

            if (requiredLink is HorizontalLink)
                RenderHorizontalLink(requiredLink);
            else
                RenderVerticalLink(requiredLink);
        }

        private void RenderHorizontalLink(Link link)
        {
            var step = link.Displacement / Math.Abs(link.Displacement);
            for (var i = 0; i != link.Displacement; i += step)
            {
                Console.SetCursorPosition(link.Node.Column + i, link.Node.Row);
                Console.Write('_');
            }
        }

        private void RenderVerticalLink(Link link)
        {
            var step = link.Displacement / Math.Abs(link.Displacement);
            for (var i = 0; i != link.Displacement; i += step)
            {
                Console.SetCursorPosition(link.Node.Column, link.Node.Row + i);
                Console.Write('|');
            }
        }

        private void RenderCountry(CountryInfo country)
        {
            var start = country.StateSpace.TopLeft;
            var end = country.StateSpace.BottomRight;
            var width = end.Column - start.Column + 1;
            var height = end.Row - start.Row;
            var marker = '\u2588';

            var horizontalBorder = String.Join("", new char[width].Select(ch => marker));
            var internalRow = String.Join("", new char[width].Select((ch, i) => i == 0 || i == width - 1 ? marker : ' '));

            //Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), country.Continent.Color);
            Console.ForegroundColor = ConsoleColor.Gray;

            // Horizontal borders
            Console.SetCursorPosition(start.Column, start.Row);
            Console.Write(horizontalBorder);

            Console.SetCursorPosition(start.Column, end.Row);
            Console.Write(horizontalBorder);

            // Vertical borders
            for (var i = 1; i < height; i++)
            {
                Console.SetCursorPosition(start.Column, start.Row + i);
                Console.Write(internalRow);
            }

            // Country Information
            RenderCountryInformation(country, country.Occupier.Color);
        }

        private void RenderCountryInformation(CountryInfo country, string color)
        {
            var start = country.StateSpace.TopLeft;
            var end = country.StateSpace.BottomRight;
            var width = end.Column - start.Column + 1;
            var height = end.Row - start.Row;

            var infoComponents = Regex.Split(country.Name.ToString(), @"(?<!^)(?=[A-Z])").ToList(); // Regex splits by capital letter
            infoComponents[0] = $"{(int)country.Name}. {infoComponents[0]}";
            infoComponents.Add($"{'\u2694'} {country.Armies.ToString().PadLeft(2, '0')}");

            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
            var informationStart = new CoOrdinate
            (
                start.Row + ((height + 1) / 2) - (infoComponents.Count / 2),
                start.Column + ((width - infoComponents.Max(e => e.Length)) / 2)
            );

            foreach (var component in infoComponents)
            {
                Console.SetCursorPosition(informationStart.Column, informationStart.Row + infoComponents.FindIndex(e => e == component));
                Console.Write(component);
            }

            // Consider handling this outside of the Render() method
            country.StateSpace.ArmyPosition = new CoOrdinate(Console.CursorTop, informationStart.Column + 2);
        }
    }
}
