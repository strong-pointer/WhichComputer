Feature: Result Retrieval
	Tests that external API calls are parsed correctly.


# I intentionally did not include live sites in the tests, as we have no control over when they might change
# and we want to avoid spamming any services when we run our code.	

@retrieval
Scenario: Retrieve five computers from Amazon successfully
	When I load the HTML located at "amazonsample.html" for the "AMAZON" service
	Then I expect the following 5 results when I fetch results for the computer "Samsung Galaxy Chromebook 2":
	| listing                                                                                                                                                                      | url                                                                                                                                                                                                                                                                                                                         | used | price  | source |
	| Samsung Galaxy Chromebook 2 XE530QDA-KA1US 13.3" Touchscreen 2 in 1 Chromebook - Full HD - 1920 x 1080 - Intel Core i3 (10th Gen) i3-10110U 2.10 GHz - 8 GB RAM - Fiesta Red | https://www.amazon.com/Samsung-Electronics-Galaxy-Chromebook-i3-Processor/dp/B097WD9P4K/ref=sr_1_1?keywords=Samsung+Galaxy+Chromebook+2&amp;qid=1678477717&amp;refinements=p_n_condition-type%3A2224373011&amp;rnid=2224369011&amp;s=electronics&amp;sr=1-1&amp;ufe=app_do%3Aamzn1.fos.2b70bf2b-6730-4ccf-ab97-eb60747b8daf | true | 670.56 | AMAZON |
	| SAMSUNG 13.3” Galaxy Chromebook Laptop Computer w/ 256GB Storage, 8GB RAM, ‎4K AMOLED Touchscreen Display, Ultra Slim Design, Chrome OS, WiFi 6, Mercury Gray                | https://www.amazon.com/SAMSUNG-Chromebook-Computer-Storage-Touchscreen/dp/B088TDDNGT/ref=sr_1_2?keywords=Samsung+Galaxy+Chromebook+2&amp;qid=1678477717&amp;refinements=p_n_condition-type%3A2224373011&amp;rnid=2224369011&amp;s=electronics&amp;sr=1-2&amp;ufe=app_do%3Aamzn1.fos.2b70bf2b-6730-4ccf-ab97-eb60747b8daf    | true | 999.00 | AMAZON |
	| SAMSUNG 13.3" Galaxy Chromebook Laptop Computer w/ 256GB Storage, 8GB RAM, 4K AMOLED Touchscreen Display, HD Intel Core I-5 Processor, Ultra Slim, US Warranty, Fiesta Red   | https://www.amazon.com/SAMSUNG-Chromebook-Computer-Touchscreen-Processor/dp/B088T2C912/ref=sr_1_3?keywords=Samsung+Galaxy+Chromebook+2&amp;qid=1678477717&amp;refinements=p_n_condition-type%3A2224373011&amp;rnid=2224369011&amp;s=electronics&amp;sr=1-3&amp;ufe=app_do%3Aamzn1.fos.2b70bf2b-6730-4ccf-ab97-eb60747b8daf  | true | 998.99 | AMAZON |
	| SAMSUNG Chromebook Plus V2, 2-in-1, 4GB RAM, 32GB eMMC, 13MP Camera, Chrome OS, 12.2", 16:10 Aspect Ratio, Light Titan (XE520QAB-K01US)                                      | https://www.amazon.com/Samsung-Chromebook-Plus-Camera-Chrome/dp/B07J1SY5QQ/ref=sr_1_4?keywords=Samsung+Galaxy+Chromebook+2&amp;qid=1678477717&amp;refinements=p_n_condition-type%3A2224373011&amp;rnid=2224369011&amp;s=electronics&amp;sr=1-4&amp;ufe=app_do%3Aamzn1.fos.f5122f16-c3e8-4386-bf32-63e904010ad0              | true | 249.99 | AMAZON |
	| SAMSUNG Galaxy Book2 Pro 13.3” 256GB Laptop Computer w/ 8GB RAM, 12th Gen Intel Core i5 Evo Certified Processor, AMOLED Screen, Long Lasting Battery, Thin Design, 2022, US Version, Silver | https://www.amazon.com/SAMSUNG-13-3-Galaxy-Book2-Silver/dp/B09R8Y6BVR/ref=sr_1_5?keywords=Samsung+Galaxy+Chromebook+2&amp;qid=1678477717&amp;refinements=p_n_condition-type%3A2224373011&amp;rnid=2224369011&amp;s=electronics&amp;sr=1-5&amp;ufe=app_do%3Aamzn1.fos.765d4786-5719-48b9-b588-eab9385652d5                   | true | 1054.16 | AMAZON |

@retrieval
Scenario: Attempt to query Amazon with an empty string
	When I use the "AMAZON" service
	Then I expect the application to throw an error when I query with ""

@retrieval
Scenario: Attempt to query Amazon with a string containing only newlines
	When I use the "AMAZON" service
	Then I expect the application to throw an error when I query with "\n\n\n\n\n\r\n"

@retrieval
Scenario: Attempt to query Amazon with a computer name that does not exist
	When I use the "AMAZON" service
	Then I expect the application to throw an error when I query with "DOES NOT EXIST"