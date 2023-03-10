#server test
#table test 
test_db = "Tutorial_test"
control_test = "Control_plan_test"

def test_db(size_testcase):
    control_db_test = []
    for i in range(size_testcase):
        control_db_test[i] = (control_test + "_" +i).ToString()
    return control_db_test


def db_connect_test(function_test, db_test):
    for i in range (db_test.size()):
        try:
            function_test(db_test[i])
        except:
            
    

