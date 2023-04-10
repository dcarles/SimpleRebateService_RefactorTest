 ## PROBLEMS:
 There are several problems with the RebateService class in terms of SOLID principles, readability, testability, and maintainability:
  
1. Single Responsibility Principle (SRP): The RebateService class violates the SRP as it performs multiple responsibilities, such as retrieving data from data stores, calculating rebate, and storing the result in the data store. This class should be split into smaller classes with single responsibilities. It should still work as a coordinator, but the responsibilities for data access and rebate calculation must be perform by other classes and these injected as interfaces

2. Open/Closed Principle (OCP): The RebateService class violates the OCP as it does not provide an easy way to add new incentive types. To support the OCP, the class should be designed to allow for extension without modification. One way to achieve this would be to use the strategy pattern, where each incentive type has its own implementation.

3. Dependency Inversion Principle (DIP): The RebateService class violates the DIP as it creates instances of the RebateDataStore and ProductDataStore classes directly, tightly coupling the class to these implementations. Instead, the class should depend on abstractions (interfaces) rather than concrete implementations.

4. Readability: The RebateService class is not very readable due to the use of switch statements and complex if-else conditions, as well as having too many lines of code in one single method. Using the strategy pattern as mentioned above would make the code more readable and easier to understand.

5. Testability: The RebateService class is difficult to test as it creates instances of the data stores directly. Instead, it should be designed to depend on abstractions (interfaces) that can be mocked or stubbed in unit tests.

6. Maintainability: The RebateService class is not very maintainable as it violates the SRP and OCP, making it difficult to extend and modify. It should be refactored into smaller classes with single responsibilities and should depend on abstractions rather than concrete implementations.
 
 
## SOLUTIONS:
 
1. Applied a Factory pattern in combination with a Strategy pattern to fix the Readability, Maintainability and comply with Open/Closed Principle. To be able to Create the different types of RebateCalculator used a Factory which would know how to create each type (based on rebate.Incentive). Then Each of the incentive types are created as a RebateCalculator implementation with CanCalculate and Calculate methods

2. Injected all Classes as Interfaces in the RebateService to make sure complies with Dependency Inversion Principle and be able to test the class.

3. Even when class is still retrieving data, saving data and calculating, it acts more like a coordinator and each of those steps are done by a different class which is injected, so it should be ok.

 
 #### SIDE NOTE:  
 I assume the `rebateAmount +=` in the original logic was a mistake as based on the logic this does not seem stackable as there are no multiple products or rebates or requests and the amount is not returned outside the method or saved anywhere. If we want to use the existing rebateAmount retrieved in db it could be possible to do in each of the calculators, but in the original logic it always start at 0 so it was not doing that.
 