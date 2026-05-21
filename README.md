<!--
Copyright (c) 2023-2026 Robert A. Howell
Author: Robert A. Howell
Description: This project was created as a portfolio piece and demonstrates web application development deployed as https://www.rh-snapi-site.com/.
Created_Date: November 2023
Edited: 2026-03-10
Updated: 2026-05-21
-->

# SpaceFlight News App  
Spaceflight News App is a website created to showcase my developer skill sets. Details are written below about the project, from its creation.  

The live project operates at [https://spaceflight-web.rhdeveloping.com/](https://spaceflight-web.rhdeveloping.com/).  

_____

**Below is an archive version of this repository**  

## 7-1-2024 Update  
This site is now live! This code repository serves to host the previous iteration of the site's live code. Today the code and code stack are very different and are not open source. The site, however, is available for public viewing and querying the database.  

## Features  
This is a blazor web application using:
1. Components implementation using razor syntax (C#, HTML, CSS, CSHTML)
2. Multi-project solution developed in Visual Studio
3. ASP.NET API endpoint:
 - Article fetch at /spaceflightAPI/articles
 - APOD fetch at /spaceflightAPI/apod
4. Bootstrap CSS
5. C# MVC
6. JSON file  (Backend)
7. API fetches of article data in JSON format (see Server/Services/JsonFileFetchService.cs) provide the content used in the web assembly page

## Database backend
![Website topology 11-17-23](WebFarm.drawio.svg)

> ![icon8 database icon](3d-fluency-database.png)
>
> Database illustration by <a href="https://icons8.com/illustrations/author/zD2oqC8lLBBA">Icons 8</a> from <a href="https://icons8.com/illustrations">Ouch!</a>
>
> ![icon8 cloudflare icon](icons8-cloudflare-48.png)
>
> <a target="_blank" href="https://icons8.com/icon/13682/cloudflare">CloudFlare</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
>
> ![icon8 article icon](icons8-article-48.png)
>
> <a target="_blank" href="https://icons8.com/icon/13620/page">Article</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
>
> ![icon8 red article icon](icons8-article-48-red.png)
>
> <a target="_blank" href="https://icons8.com/icon/13119/news">Article</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
>
> Remaining icons from <a target="_blank" href="https://app.diagrams.net/">draw.io</a>  
> Diagram created at <a target="_blank" href="https://app.diagrams.net/">draw.io</a>  
