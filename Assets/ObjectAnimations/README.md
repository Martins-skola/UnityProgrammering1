# Enkla matematiska animering av objekt
Du kan skapa många olika enklare symetriska animeringar med enkel matematik. Komponent-skripten nedan är sådana exempel, och scenen `SCENE_ObjectAnimationsSimple` innehåller exempel där skripten används.

Samtliga skript nedan erbjuder valet utefter vilken axel animeringen ska ske. Skapar du en animeringskomponent för ett speciellt syfter eller typ av gameobjects, kan du hårdkoda animerings-axel.
Skripten förflyttar också obkjekten med `Transform`-komponenten, men det kan du modifiera så att förflyttningen t.ex. sker med `CharacterController`-komponenten, så att objektets förflyttning även kollisionkontrolleras.


## Automatisk rotation
Skriptet `AutoRotateAnimation` skapar en komponent som **roterar ett objekt** med viss *hastighet* runt en angiven axel.

## Ping-pong-förflyttning ("wobble") med Cosinus-funktion
Skriptet `CosinusWobbleAnimation` **flyttar ett objekt** fram och tillbaka längs en angiven axel, med en viss *amplitud (längd)* och *frekvens (hastighet)*, objektet "wobblar" (skakar).

Genom att hämta längden som objektet ska förflyttas längs axeln varje frame från en Cosinus-funktion, får man en mjuk rörelse där inbromsning- och acceleration sker vid ändpunkterna.
Att använda Cosinus för att få "mjukhet" i slutet på värde-ändpunkter är en vanlig metodik i spelprogrammering.
![Cosinus-funktion](https://www.intmath.com/trigonometric-graphs/svg/svgphp-graphs-sine-cosine-amplitude-1-s0.svg)

## Cirkulär rörelse runt en centrumpunkt
Skriptet `CosinusWobbleAnimation` flyttar ett objekt runt en centrumpunkt, så att objekt rör sig i en **cirkulär rörelse**, med en viss *hastighet* och *radie* från centrumpunkten.
Beräkningen av positioner för förflyttningen sker med trigonometri och enhetscirkeln (en rätvinklig triangel i enhetcirkeln), så *hastighet* är praktiken en **vinkelhastighet** (vinkel/sekund) beskriven i radianer.
Du kan förstås modifera detta så hastigheten anges i grader, eller till och med en förflyttningshastighet (som du då måste konvertera till en vinkelhastighet på något sätt)
![enhetscikeln med rätvinklig triangel](https://eddler.se/wordpress/wp-content/uploads/Enhetscirkeln-3t.png)
