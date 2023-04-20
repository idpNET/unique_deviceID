# Unique device ID Generator  
(based on Hardware and software specs) + PBKDF2_hashing
    
    This is a C# console app.
    First step is to run the project!
    Then you need to choose either FULL (Both hardware and software specs) or SEMI (only hardware specs) mode.
    Results: Computed hash value of device ID + how much hashing process took
    NOTE! Salting must be static to generate a fixed unique hash value of the device ID. By default, salt value is set to "DEFAULT".


