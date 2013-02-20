//
//  MpiData.cpp
//  MPI_DataVisualizer
//
//  Created by Patricio Gonzalez Vivo on 2/19/13.
//
//

#include "MpiData.h"

MpiData::MpiData(){
    
}

void MpiData::loadCities( string _cvsFile ){
    ofxCsv  cvs;
    cvs.loadFile( ofToDataPath( _cvsFile ) );
    
    //  Check every row except for the first one that have the titles of the columns
    //
    for (int i = 1; i < cvs.data.size(); i++){
        CityLoc newCity;
        newCity.nId = cvs.getInt(i, 0);
        
        newCity.name = cvs.getString(i, 1);
        newCity.state = cvs.getString(i, 2);
        
        newCity.latitud = cvs.getFloat(i, 3);
        newCity.longitud = cvs.getFloat(i, 4);
        int stars = cvs.getInt(i, 5);
        
        switch (stars) {
            case 1:
                newCity.stars = LARGEST_CONCENTRATION;
                break;
            case 2:
                newCity.stars = FASTEST_GROWING;
                break;
            case 3:
                newCity.stars = ACTIVE_RECRUITING;
                break;
        }
        
        cout << newCity.name << ", " << newCity.state << " (" << newCity.latitud << "," << newCity.longitud << ")" << endl;
        
        cities.push_back(newCity);
    }
}

void MpiData::loadSample( int _year, string _cvsFile ){
    
    ofxCsv  cvs;
    cvs.loadFile( ofToDataPath( _cvsFile ) );
    
    vector<CitySample> newYearSample;
    
    //  Check every row except for the first one that have the titles of the columns
    //
    for (int i = 1; i < cvs.data.size(); i++){
        
        CitySample newSample;
        newSample.nId = cvs.getInt(i, 0);
        newSample.pop = cvs.getInt(i, 2);               //Number: Total pop
        newSample.popImm = cvs.getInt(i, 3);            //Number:  Immigrants
        newSample.popImmShare = cvs.getFloat(i, 4);     //Immigrant share (%) (???)
        newSample.recArr = cvs.getFloat(i, 5);          //Recent arrivals (%) of all immigrants
        
        //  Education
        //
        newSample.noDegreeImm = cvs.getFloat(i, 6);    //Percent: No high school degree   Immigrants
        newSample.hsDegreeImm = cvs.getFloat(i, 7);    //Percent: High school/AA degree  Immigrants
        newSample.baDegreeImm = cvs.getFloat(i, 8);    //Percent: BA/higher  Immigrants
        newSample.noDegreeNat = cvs.getFloat(i, 9);    //Percent: No high school degree  Native born
        newSample.hsDegreeNat = cvs.getFloat(i, 10);    //Percent: High school/AA degree Native born
        newSample.baDegreeNat = cvs.getFloat(i, 11);    //Percent: BA/higher Native born
        
        //  Employment
        //
        newSample.employedTotal = cvs.getInt(i, 12);  //Number:  Employed
        newSample.employedImm = cvs.getInt(i, 13);    //Number:  Imm  Employed
        newSample.employedNat = cvs.getInt(i, 14);    //Number:  Natives  Employed
        
        newSample.employedImmShare = cvs.getFloat(i, 15);   // Imm share among all empl (%) (???)
        newSample.unEmployment = cvs.getFloat(i, 16);       //Unemployment rate (%)
        
        //  Financial
        //
        newSample.poverty = cvs.getFloat(i, 17);        //Poverty rate (%)
        newSample.homeOwners = cvs.getFloat(i, 18);     //Rate of home ownership (%)
        
        //  Ethnic/Cultural
        //
        newSample.black = cvs.getFloat(i, 19);          //Percent: Black
        newSample.asian = cvs.getFloat(i, 20);          //Percent: Asian
        newSample.latino = cvs.getFloat(i, 21);         //Percent: Latino
        newSample.nonWhite = cvs.getFloat(i, 22);       //Percent: Non-white
        newSample.nonEnglSpk = cvs.getFloat(i, 23);     //Percent: Speak other lang (than English)
        
        newSample.creativeClass = cvs.getFloat(i, 24);
        
        newYearSample.push_back(newSample);
    }
    
    years.push_back(_year);
    samples.push_back(newYearSample);
}

void MpiData::interpolateDataBase(){
    
}

