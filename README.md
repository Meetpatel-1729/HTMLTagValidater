# HTMLTagValidater
It validates the HTML tag which uses C#, Stack and a List

## Implementation
- Created a list of self closing tags such as <img>, <link>
- Used Stack as it is a FIFO (First In First Out) data structure which can be uswefull to valdiate HTML tags
- Whenever it finds an opening tag it will push it into stack and when it find a closing bracket it will Pop that tag from the Stack
- So if the closing tag is missing then it will tell the user which tag is missing
