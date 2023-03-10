import function.function_cefile  as fc_ce
import function.function_xml as fc_xml
from database.raw_data_string import *

def main():
    df = fc_ce.read_excel(excel_file)
    data = fc_ce.read_sheet(df)
    fc_xml.parse_para_xml(xml_globals, data)
    fc_xml.parse_para_xml(xml_recipe, data)
    fc_xml.color_cell(data)
    fc_ce.save(df)

if __name__ == "__main__":
    main()

## python3 main/__main_file__.py


