
## function compare by tranfer PLC data to Natural Languge data
def compare(value_por, value_actual, priority):
    str_temp = value_por.value
    if (str(value_por.value) == str(value_actual.value))\
        or (str(value_por.value) == 'Check' and str(value_actual.value) == '1')\
        or (str(value_por.value) == 'check' and str(value_actual.value) == '1')\
        or (str(value_por.value) == 'uncheck' or str(value_por.value) == 'Uncheck' ):
        return 1
    elif (str(value_por.value) == 'On' and str(value_actual.value) =='1')\
        or (str(value_por.value) == 'ON' and str(value_actual.value) =='1')\
        or (str(value_por.value) == 'on' and str(value_actual.value) =='1')\
        or (str(value_por.value) == 'Off' and str(value_actual.value) =='0')\
        or (str(value_por.value) == 'OFF' and str(value_actual.value) =='0')\
        or (str(value_por.value) == 'off' and str(value_actual.value) =='0')\
        or (str(value_por.value) == 'True' and str(value_actual.value) =='1') \
        or (str(value_por.value) =='False' and str(value_actual.value) =='0') \
        or (str(value_por.value) == 'TRUE' and str(value_actual.value) =='1') \
        or (str(value_por.value) == 'true' and str(value_actual.value) =='1') \
        or (str(value_por.value) == 'false' and str(value_actual.value) =='0') \
        or (str(value_por.value) == 'false' and str(value_actual.value) =='0') \
        or (str(value_actual.value) == None)\
        or (str(str_temp).replace(",", ".") == str(value_actual.value)): 
        return 1
    elif priority.value == 'A':
        return 1
    else:
        return 0

def compare_string(value_por, value_actual, priority):
    str_temp = value_por
    if (str(value_por) == str(value_actual))\
    or (str(value_por) == 'Check' and str(value_actual) == '1')\
    or (str(value_por) == 'check' and str(value_actual) == '1')\
    or (str(value_por) == 'uncheck' or str(value_por) == 'Uncheck' ):
        return 1
    elif (str(value_por) == 'On' and str(value_actual) =='1')\
        or (str(value_por) == 'ON' and str(value_actual) =='1')\
        or (str(value_por) == 'on' and str(value_actual) =='1')\
        or (str(value_por) == 'Off' and str(value_actual)=='0')\
        or (str(value_por) == 'OFF' and str(value_actual) =='0')\
        or (str(value_por) == 'off' and str(value_actual)=='0')\
        or (str(value_por) == 'True' and str(value_actual)=='1') \
        or (str(value_por) =='False' and str(value_actual)=='0') \
        or (str(value_por) == 'TRUE' and str(value_actual)=='1') \
        or (str(value_por) == 'true' and str(value_actual)=='1') \
        or (str(value_por) == 'false' and str(value_actual) =='0') \
        or (str(value_por) == 'false' and str(value_actual) =='0') \
        or (str(value_actual) == None)\
        or (str(str_temp).replace(",", ".") == str(value_actual)): 
        return 1
    elif value_actual.find(value_por) != -1:
        return 1
    elif value_por.lower() == value_actual.lower():
        return 1
    elif priority == 'A':
        return 1
    elif value_por == "None":
        return 1
    else:
        return 0