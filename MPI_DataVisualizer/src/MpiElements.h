//
//  MpiElements.h
//  MPI_DataVisualizer
//
//  Created by Patricio Gonzalez Vivo on 2/19/13.
//
//

#ifndef MPI_DataVisualizer_MpiElements_h
#define MPI_DataVisualizer_MpiElements_h

enum mpiStarsIndex {
    LARGEST_CONCENTRATION,  //  1   Ten cities with largest concentration of foreing-born popultion
    FASTEST_GROWING,        //  2   Ten cities with fastest growing foreing-born population
    ACTIVE_RECRUITING       //  3   Ten cities most actively cruiting the foreing born
    
    //  Se the end of https://github.com/patriciogonzalezvivo/DataToys/blob/master/MPI_DataBase/project_overview.pdf?raw=true
    //
};

//  GeoLocalization information of the city
//
struct CityLoc{
    int     nId;            //  We need this to cross the information with the Sample to place it in a map
    
    string  name, state;        //  It's good to have names isn' t
    float   latitud, longitud;  //  This is the key to position them on a world map
    
    mpiStarsIndex stars;    //  Using the Index described here https://github.com/patriciogonzalezvivo/DataToys/blob/master/MPI_DataBase/project_overview.pdf?raw=true
};

//  Sample by City
//
struct CitySample {
    
    //  City ID
    //
    int     nId;
    
    //  Population
    //
    int     pop;            //Number: Total pop
    int     popImm;         //Number:  Immigrants
    
    float   popImmShare;    //Immigrant share (%)
    float   recArr;         //Recent arrivals (%) of all immigrants
    
    //  Education
    //
    float   noDegreeImm;    //Percent: No high school degree   Immigrants
    float   hsDegreeImm;    //Percent: High school/AA degree  Immigrants
    float   baDegreeImm;    //Percent: BA/higher  Immigrants
    float   noDegreeNat;    //Percent: No high school degree  Native born
    float   hsDegreeNat;    //Percent: High school/AA degree Native born
    float   baDegreeNat;    //Percent: BA/higher Native born
    
    //  Employment
    //
    int     employedTotal;  //Number:  Employed
    int     employedImm;    //Number:  Imm  Employed
    int     employedNat;    //Number:  Natives  Employed
    
    float   employedImmShare;   // Imm share among all empl (%)
    float   unEmployment;       //Unemployment rate (%)
    
    //  Financial
    //
    float   poverty;        //Poverty rate (%)
    float   homeOwners;     //Rate of home ownership (%)
    
    //  Ethnic/Cultural
    //
    float   black;          //Percent: Black
    float   asian;          //Percent: Asian
    float   latino;         //Percent: Latino
    float   nonWhite;       //Percent: Non-white
    float   nonEnglSpk;     //Percent: Speak other lang (than English)
    
    float   creativeClass;  //Percent: Creative class
    
    //  Confused?? Need help?? Check this:
    //  https://github.com/patriciogonzalezvivo/DataToys/blob/master/MPI_DataBase/data_overview.pdf?raw=true
    
};

#endif
